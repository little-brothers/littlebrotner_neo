using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class Television : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {
	[Serializable]
	struct Alarm
	{
		public Sprite texture;
		public Color color;
	}

	[SerializeField]
	int id;

	[SerializeField]
	Sprite background_image;

	[SerializeField]
	Sprite effect_image;

	[SerializeField]
	Sprite symbol_image;

	[SerializeField]
	GameObject noti_image;

	[SerializeField]
	GameObject detailView;

	[SerializeField]
	string hint;

	[SerializeField]
	AudioSource soundFx;

	[SerializeField]
	AudioClip click;

	[SerializeField]
	AudioClip hover;

	[SerializeField]
	Alarm AlarmNoti;
	[SerializeField]
	Alarm AlarmWarn;
	[SerializeField]
	Alarm AlarmAlert;



	SpriteRenderer _background;
	SpriteRenderer _effect;
	SpriteRenderer _symbol;
	SpriteRenderer _alarm;
	EnergyManager _energyManager;
	GameObject _overlay;
	int _eventDay = 0; // 마지막으로 이벤트가 있었던 날짜
	int _disasterDay = 0; // 마지막으로 재난이 일어난 날짜
	Alarm _defaultAlarm;

	//GameObject _noti;

	// for 03 screen
	List<Item> productCache = new List<Item>();

	bool _watched = false;
	public bool watched {
		get { return _watched; }
		private set {
			_watched = value;
			UpdateBrightness();
		}
	}

	void Awake()
	{
		_background = GetComponent<SpriteRenderer>();
		_effect = transform.Find("Effect").GetComponent<SpriteRenderer>();
		_symbol = transform.Find("Symbol").GetComponent<SpriteRenderer>();
		_energyManager = GameObject.FindObjectOfType<EnergyManager>();
		_overlay = transform.Find("Overlay").gameObject;
		_alarm = transform.Find("Alarm").GetComponent<SpriteRenderer>();
	}

	// Use this for initialization
	void Start () {
		_background.sprite = background_image;
		_effect.sprite = effect_image;
		_symbol.sprite = symbol_image;

		// 다음날 되면 본것들 저리
		MyStatus.instance.AddSleepHook(ResetStatus);

		// 위기상항 알림 1 - 모래폭풍
		MyStatus.instance.homeDestroyed.OnUpdate += destoryed => {
			if (destoryed != 0) {
				_disasterDay = destoryed;
			}
		};

		// 위기상황 알림 2 - 침공
		MyStatus.instance.invasion.OnUpdate += invade => {
			if (invade != 0) {
				_disasterDay = MyStatus.instance.day + 1;
			}
		};

		_overlay.transform.Find("Hint").GetComponent<TextMesh>().text = hint;
		_overlay.SetActive(false);
		watched = false;

		SetupListenersForEachTV();
	}

	void SetAlarmType(Alarm alarm, bool setDefault=true)
	{
		_alarm.sprite = alarm.texture;
		_alarm.color = alarm.color;

		if (setDefault)
			_defaultAlarm = alarm;
	}

	void SetupListenersForEachTV()
	{
		// 
		switch (id)
		{
		case 1:
			SetAlarmType(AlarmNoti);
			MyStatus.instance.AddSleepHook((vote, status, noti) => {

			});
			break;

		case 2:
			SetAlarmType(AlarmWarn);
			MyStatus.instance.economy.OnUpdate += eco => {
				if (Mathf.Abs(eco) >= 70f)
				{
					_eventDay = MyStatus.instance.day+1;
					UpdateAlarm(MyStatus.instance.day+1);
				}
			};
			break;

		case 3:
			SetAlarmType(AlarmNoti);

			MyStatus.DataUpdateNotifier<int>.DataUpdatedEvent checkProducts = day => {
				bool hasNew = false;
				var prevProducts = productCache;
				var nowProducts = TV03.availableItems.ToList();

				foreach (var product in nowProducts)
				{
					int idx = prevProducts.FindIndex(p => p.id == product.id);
					if (idx < 0)
					{
						hasNew = true;
						break;
					}
				}

				if (hasNew)
				{
					_eventDay = day;
					UpdateAlarm(day);
				}

				productCache = nowProducts;
			};

			MyStatus.instance.day.OnUpdate += checkProducts;
			checkProducts(MyStatus.instance.day);
			break;

		case 5:
			SetAlarmType(AlarmNoti);

			// 매일매일 표시
			MyStatus.DataUpdateNotifier<int>.DataUpdatedEvent checkVote = day => {
				_eventDay = day;
				UpdateAlarm(day);
			};

			MyStatus.instance.day.OnUpdate += checkVote;

			// 기본으로 하나 표시해줌
			checkVote(MyStatus.instance.day);
			break;

		case 7:
			SetAlarmType(AlarmNoti);
			MyStatus.DataUpdateNotifier<int>.DataUpdatedEvent checkHisotry = day => {
				History current = Database<History>.instance.Find(day);
				if (current.script != "")
				{
					_eventDay = day;
					UpdateAlarm(day);
				}
			};

			MyStatus.instance.day.OnUpdate += checkHisotry;

			// 처음에도 한번 체크
			checkHisotry(MyStatus.instance.day);
			break;

		case 8:
			SetAlarmType(AlarmWarn);
			MyStatus.instance.political.OnUpdate += pol => {
				if (Mathf.Abs(pol) >= 70f)
				{
					_eventDay = MyStatus.instance.day+1;
					UpdateAlarm(MyStatus.instance.day+1);
				}
			};
			break;

		case 9:
			SetAlarmType(AlarmNoti);
			MyStatus.instance.technologies.OnUpdate += tech => {
				// 기술은 잠잘때만 업데이트됨
				_eventDay = MyStatus.instance.day+1;
				UpdateAlarm(MyStatus.instance.day+1);
			};
			break;

		default:
			break;
		}

		UpdateAlarm(MyStatus.instance.day);
	}

	void OnEnable()
	{
		// 브라운관 tv처럼 보이게 하려고 hue 주기적으로 업데이트
		// deactivate되면 코루틴이 꺼지게 되므로 OnEnable에서 처리한다
		StartCoroutine(UpdateHue());

		if (_disasterDay == MyStatus.instance.day)
		{
			_alarm.gameObject.SetActive(true); // always show
			SetAlarmType(AlarmAlert, false);
			GetComponent<Animation>().Play("blink_alert", PlayMode.StopAll);
		}
		else
		{
			GetComponent<Animation>().Play("blink", PlayMode.StopAll);
		}
	}

	void OnDisable()
	{
		_overlay.SetActive(false);
	}

	// invoked from blink_alert animation
	public void ResetAlert()
	{
		GetComponent<Animation>().Play("blink");
		SetAlarmType(_defaultAlarm, false);
		UpdateAlarm(MyStatus.instance.day);
	}

	public void HoverSound(){

	//	Debug.Log ("SOUND!");
		soundFx.PlayOneShot (hover);

	}

	public void ClickSound(){

	//	Debug.Log ("SOUND!");
		soundFx.PlayOneShot (click);

	}



    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {

		ClickSound ();

		// 에너지 사용 체크
		if (!watched && !_energyManager.UseEnergy())
			return;

		ShowFullscreen();
    }

	public void ShowFullscreen()
	{
		watched = true;

		// reset alarm
		_eventDay = -1;
		UpdateAlarm(MyStatus.instance.day);

		if (detailView != null)
		{
			// show 
			var view = GameObject.Instantiate(detailView);
			Utilities.SetUIParentFit(GameObject.FindWithTag("RootCanvas"), view);
			view.transform.position += new Vector3(0, 0, -5);
		}
	}

	void UpdateBrightness()
	{
		float brightness = watched ? 1.0f : 0.1f;
		foreach(SpriteRenderer r in new SpriteRenderer[]{_background, _effect, _symbol})
		{
			var color = r.material.color;
			color.r = color.g = color.b = brightness;
			r.material.color = color;
		}
	}

	public void ResetStatus(Vote vote, MyStatus.Snapshot status, List<Notification> noti)
	{
		watched = false;
		UpdateAlarm(status.day+1);
	}

	void UpdateAlarm(int day)
	{
		_alarm.gameObject.SetActive(_eventDay == day);
	}

	IEnumerator UpdateHue()
	{
		const float shiftRange = 0.04f;
		while(true)
		{
			yield return new WaitForSeconds(0.4f + UnityEngine.Random.value * 0.1f);
			float hueshift = (UnityEngine.Random.value - 0.5f) * shiftRange;
			foreach(SpriteRenderer r in new SpriteRenderer[]{_background, _effect, _symbol})
				r.material.SetFloat("_HueShift", hueshift);
		}
	}

	void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
	{
		_overlay.SetActive(false);
	}

	void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
	{
		_overlay.SetActive(true);
		HoverSound ();
	}
}
