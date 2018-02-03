using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopListElement : ListElementBase, IPointerClickHandler {
	Item _product;
	public Item product {
		set {
			_product = value;
			_price.text = _product.price.ToString();
			_name.text = _product.name;
		}
	}

	Text _price;
	Text _name;

	void Awake()
	{
		_price = transform.Find("Price").GetComponent<Text>();
		_name = transform.Find("Name").GetComponent<Text>();
	}

	public void OnBuyButton()
	{
		if (MyStatus.instance.money >= _product.price) {
			ConfirmPopup.Setup(string.Format("Are you sure to buy '{0}'?", _product.name), () => {
				MyStatus.instance.money.value -= _product.price;
				// TODO 아이템 효과
			});
		}
	}

	void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
	{
		OnBuyButton();
	}
}
