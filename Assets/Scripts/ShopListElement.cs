using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopListElement : ListElementBase, IPointerClickHandler {
	public delegate void OnItemSelectedEvent(Item item);
	public event OnItemSelectedEvent OnItemSelected;

	Item _product;
	public Item product {
		set {
			_product = value;
			_price.text = _product.price.ToString();
			_name.text = _product.name;
			_icon.sprite = null;
			if (value.id <= productIcons.Length) {
				_icon.sprite = productIcons[value.id-1];
			}
		}
	}

	Text _price;
	Text _name;
	Image _icon;

	[SerializeField]
	Sprite[] productIcons;

	void Awake()
	{
		_price = transform.Find("Price").GetComponent<Text>();
		_name = transform.Find("Name").GetComponent<Text>();
		_icon = transform.Find("Icon").GetComponent<Image>();
	}

	public void OnBuyButton()
	{
		OnItemSelected(_product);
	}

	void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
	{
		OnBuyButton();
	}
}
