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

	public static IEnumerable<Item> availableItems 
	{
		get {
			return Database<Item>.instance.ToList().Where(byVisibility);
		}
	}

	static bool byVisibility(Item item)
	{
		foreach(var condition in item.showConditions) {
			if (!MyStatus.Check(condition))
				return false;
		}

		return true;
	}

	void Start () {
		Utilities.SetUIParentFit(GameObject.FindGameObjectWithTag("RootCanvas"), gameObject);

		_money = transform.Find("Money").GetComponent<Text>();
		MyStatus.instance.money.OnUpdate += updateMoney;
		updateMoney(MyStatus.instance.money);

		var listElemTmpl = Resources.Load<GameObject>("ShopListElement");
		foreach (var item in availableItems)
		{
			var elem = GameObject.Instantiate(listElemTmpl, list.transform).GetComponent<ShopListElement>();
			elem.product = item;
			elem.OnItemSelected += OnItemSelected;
		}
	}

	void OnDestroy()
	{
		MyStatus.instance.money.OnUpdate -= updateMoney;
	}

	void OnItemSelected(Item item)
	{
		bool buyable = false;
		if (item.currency == Currency.Gold) {
			buyable = MyStatus.instance.money >= item.price;
		} else if (item.currency == Currency.Electrocity) {
			buyable = MyStatus.instance.energy >= item.price;
		}

		if (buyable) {
			ConfirmPopup.Setup(string.Format("Are you sure to buy '{0}'?", item.name), () => {
				if (item.currency == Currency.Gold) {
					MyStatus.instance.money.value -= item.price;
				} else if (item.currency == Currency.Electrocity) {
					MyStatus.instance.energy.value -= item.price;
				}

				switch (item.id) {
				case 1:
					MyStatus.instance.energy.value += 1;
					break;

				case 8:
					MyStatus.instance.money.value += 2;
					break;

				// 일반 아이템
				default:
					MyStatus.instance.inventory.Put(item);
					break;
				}
			});
		} else {
			// 살 수 없는 효과?
		}
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
