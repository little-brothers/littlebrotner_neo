using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TV03 : MonoBehaviour {
	Text _money;

	[SerializeField]
	ShopListElement.Product[] products;

	[SerializeField]
	VerticalLayoutGroup list;

	void Start () {
		Utilities.SetUIParentFit(GameObject.FindGameObjectWithTag("RootCanvas"), gameObject);

		_money = transform.Find("Money").GetComponent<Text>();
		MyStatus.instance.money.OnUpdate += updateMoney;
		updateMoney(MyStatus.instance.money);

		var listElemTmpl = Resources.Load<GameObject>("ShopListElement");
		foreach (var product in products)
		{
			var elem = GameObject.Instantiate(listElemTmpl, list.transform).GetComponent<ShopListElement>();
			elem.product = product;
		}
	}

	void updateMoney(int value)
	{
		_money.text = value.ToString();
	}
	
}
