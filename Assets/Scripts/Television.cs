using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Television : MonoBehaviour {

	[SerializeField]
	Sprite background_image;

	[SerializeField]
	Sprite effect_image;

	[SerializeField]
	Sprite symbol_image;

	SpriteRenderer _background;
	SpriteRenderer _effect;
	SpriteRenderer _symbol;

	// Use this for initialization
	void Start () {
		_background = transform.Find("Background").GetComponent<SpriteRenderer>();
		_effect = transform.Find("Effect").GetComponent<SpriteRenderer>();
		_symbol = transform.Find("Symbol").GetComponent<SpriteRenderer>();

		if (_background != null)
			_background.sprite = background_image;
		if (_effect != null)
			_effect.sprite = effect_image;
		if (_symbol != null)
			_symbol.sprite = symbol_image;
	}
}
