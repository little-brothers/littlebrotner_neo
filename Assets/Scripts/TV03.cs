using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TV03 : MonoBehaviour, ISubscribe {
	Text _money;

	[SerializeField]
	VerticalLayoutGroup list;

	void Start () {
		Utilities.SetUIParentFit(GameObject.FindGameObjectWithTag("RootCanvas"), gameObject);

		_money = transform.Find("Money").GetComponent<Text>();
		MyStatus.instance.money.OnUpdate += updateMoney;
		updateMoney(MyStatus.instance.money);

		var listElemTmpl = Resources.Load<GameObject>("ShopListElement");
		var availableItems = Database<Item>.instance.ToList().Where(byVisibility);
		foreach (var item in availableItems)
		{
			var elem = GameObject.Instantiate(listElemTmpl, list.transform).GetComponent<ShopListElement>();
			elem.product = item;
		}
	}

	void OnDestroy()
	{
		MyStatus.instance.money.OnUpdate -= updateMoney;
	}

	bool byVisibility(Item item)
	{
		foreach(var condition in item.showConditions) {
			if (!MyStatus.Check(condition))
				return false;
		}

		return true;
	}

	void updateMoney(int value)
	{
		_money.text = value.ToString();
	}

	void ISubscribe.OnNotifty(object[] values)
    {
		string eventName = values[0] as string;
		
		switch(eventName)
		{
			case EventNames.TurnOffTV:
				Destroy(this.gameObject);
				break;
		}
    }

}
