using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class Television : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

	[SerializeField]
	Sprite background_image;

	[SerializeField]
	Sprite effect_image;

	[SerializeField]
	Sprite symbol_image;

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




	SpriteRenderer _background;
	SpriteRenderer _effect;
	SpriteRenderer _symbol;
	EnergyManager _energyManager;
	GameObject _overlay;
	bool _watched = false;
	public bool watched {
		get { return _watched; }
		private set {
			_watched = value;
			UpdateBrightness();
		}
	}

	// Use this for initialization
	void Start () {
		_background = GetComponent<SpriteRenderer>();
		_effect = transform.Find("Effect").GetComponent<SpriteRenderer>();
		_symbol = transform.Find("Symbol").GetComponent<SpriteRenderer>();
		_energyManager = GameObject.FindObjectOfType<EnergyManager>();
		_overlay = transform.Find("Overlay").gameObject;

		if (_background != null)
			_background.sprite = background_image;
		if (_effect != null)
			_effect.sprite = effect_image;
		if (_symbol != null)
			_symbol.sprite = symbol_image;

		// 다음날 되면 본것들 저리
		MyStatus.instance.AddSleepHook((vote, status) => ResetWatched());

		_overlay.transform.Find("Hint").GetComponent<TextMesh>().text = hint;
		_overlay.SetActive(false);
		watched = false;
	}

	void OnEnable()
	{
		// 브라운관 tv처럼 보이게 하려고 hue 주기적으로 업데이트
		// deactivate되면 코루틴이 꺼지게 되므로 OnEnable에서 처리한다
		StartCoroutine(UpdateHue());
	}

	void OnDisable()
	{
		_overlay.SetActive(false);
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

		watched = true;

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

	public void ResetWatched()
	{
		watched = false;
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
