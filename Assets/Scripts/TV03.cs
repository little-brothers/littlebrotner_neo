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
		_money.text = "Select a item";
		MyStatus.instance.money.OnUpdate += updateMoney;
		//updateMoney(MyStatus.instance.money);

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
			var count = MyStatus.instance.inventory.slot.Count;
			if (!item.installable && count == 8) {
				buyable = false;
				_money.text = "cabinet is full!";
			}
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
					_money.text = "charged 1 energy";
					break;

				case 8:
					MyStatus.instance.money.value += 2;
					_money.text = "energy back in money";
					break;

				// 일반 아이템
				default:
					MyStatus.instance.inventory.Put(item);
					if(item.installable){
					_money.text = "Installation successful!";
					}
					else{
					_money.text = "stored in the cabinet";
					}

					break;
				}
			});
		} else {
			if( MyStatus.instance.money < item.price)
				_money.text = "not enough money";

		}
	}

	void updateMoney(int value)
	{
		//_money.text = value.ToString();
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
