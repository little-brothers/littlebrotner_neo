using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopListElement : MonoBehaviour {
	[Serializable]
	public struct Product {
		public Texture2D icon;
		public string name;
		public int price;
	}

	Text _price;
	Text _name;

	void Awake()
	{
		_price = transform.Find("Price").GetComponent<Text>();
		Debug.Assert(_price != null);

		_name = transform.Find("Name").GetComponent<Text>();
		Debug.Assert(_name != null);
	}

	public void Setup(Product product)
	{
		_price.text = product.price.ToString();
		_name.text = product.name;
	}
}
