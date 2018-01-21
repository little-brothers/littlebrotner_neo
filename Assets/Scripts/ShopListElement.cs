using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopListElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	[Serializable]
	public struct Product {
		public Texture2D icon;
		public string name;
		public int price;
	}

	Product _product;
	public Product product {
		set {
			_product = value;
			_price.text = _product.price.ToString();
			_name.text = _product.name;
			updateBuyable(MyStatus.instance.money);
		}
	}

	Text _price;
	Text _name;
	Button _buyButton;

	void Awake()
	{
		_price = transform.Find("Price").GetComponent<Text>();
		_name = transform.Find("Name").GetComponent<Text>();
		_buyButton = transform.Find("BuyButton").GetComponent<Button>();

		OnPointerExit(null);

		MyStatus.instance.money.OnUpdate += updateBuyable;
	}

	void updateBuyable(int money)
	{
		_buyButton.interactable = money >= _product.price;
	}

	public void OnBuyButton()
	{
		ConfirmPopup.Setup(string.Format("Are you sure to buy '{0}'?", _product.name), () => {
			MyStatus.instance.money.value -= _product.price;
			// TODO 아이템 효과
		});
	}

    public void OnPointerExit(PointerEventData eventData)
    {
		_price.gameObject.SetActive(true);
		_buyButton.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
    	_price.gameObject.SetActive(false);
    	_buyButton.gameObject.SetActive(true);
    }
}
