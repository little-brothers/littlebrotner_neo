using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BoxCollider2D))]
public class TV02 : MonoBehaviour, IPointerDownHandler, ISubscribe {

	float handOffset = 30f;

	Transform _arm;
	float _angle = 0;

	[SerializeField]
	GameObject _coin;
	[SerializeField]
	GameObject _food;

	// Use this for initialization
	void Start () {
		NotifyManager.Subscribe(this);

		_arm = transform.Find("arm");
		UpdateBalance(MyStatus.instance.economy);
		MyStatus.instance.economy.OnUpdate += UpdateBalance;
		Utilities.SetUIParentFit(GameObject.FindGameObjectWithTag("RootCanvas"), gameObject);

		if (MyStatus.instance.lastGift == 0 ||  MyStatus.instance.day >= MyStatus.instance.lastGift.value + 5) {
			if (MyStatus.instance.economy <= 0) {
				_coin.SetActive(false);
				_food.SetActive(true);
			} else {
				_coin.SetActive(true);
				_food.SetActive(false);
			}
		} else {
			_coin.SetActive(false);
			_food.SetActive(false);
		}
	}

	void UpdateBalance(int value)
	{
		const int maxValue = 100;
		const float maxAngle = 150.0f;
		_angle = (value / (float)maxValue) * maxAngle;
	}

	void OnEnable() {
		StartCoroutine(Swing());
	}

	void OnDestroy()
	{
		NotifyManager.UnSubscribe(this);
		MyStatus.instance.economy.OnUpdate -= UpdateBalance;
	}

	IEnumerator Swing() {
		while(true)
		{
			if (_arm == null)
			{
				yield return null;
				continue;
			}

			// 조금씩 흔들리게 하는 효과
			float fuzzyAngle = _angle + Mathf.Sin(Time.time) * 5;

			// rotate arm
			_arm.localRotation = Quaternion.AngleAxis(fuzzyAngle, Vector3.forward);

			// unrotate child of arm
			for (int i = 0; i < _arm.childCount; ++i)
			{
				_arm.GetChild(i).rotation = Quaternion.identity;
				_arm.GetChild(i).GetChild(0).localPosition = Vector3.down * handOffset;
			}

			yield return new WaitForSeconds(0.5f);
		}
	}

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
		GameObject.Destroy(gameObject);
    }

    void ISubscribe.OnNotifty(object[] values)
    {
        string eventName = values[0] as string;
		
		switch(eventName)
		{
			case EventNames.TurnOffTV:
				Destroy(gameObject);
				break;
		}
    }

	public void selection(string type)
	{
		if (type == "money") {
			MyStatus.instance.money.value += 1;
		} else {
			var food = Database<Item>.instance.Find(2);
			MyStatus.instance.inventory.Put(food);
		}

		_coin.SetActive(false);
		_food.SetActive(false);
		MyStatus.instance.lastGift.value = MyStatus.instance.day;
	}
}
