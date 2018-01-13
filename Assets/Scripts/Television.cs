using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class Television : MonoBehaviour, IPointerClickHandler {

	[SerializeField]
	Sprite background_image;

	[SerializeField]
	Sprite effect_image;

	[SerializeField]
	Sprite symbol_image;

	[SerializeField]
	GameObject detailView;

	SpriteRenderer _background;
	SpriteRenderer _effect;
	SpriteRenderer _symbol;

	// Use this for initialization
	void Start () {
		_background = GetComponent<SpriteRenderer>();
		_effect = transform.Find("Effect").GetComponent<SpriteRenderer>();
		_symbol = transform.Find("Symbol").GetComponent<SpriteRenderer>();

		if (_background != null)
			_background.sprite = background_image;
		if (_effect != null)
			_effect.sprite = effect_image;
		if (_symbol != null)
			_symbol.sprite = symbol_image;
	}

	void OnEnable()
	{
		// 브라운관 tv처럼 보이게 하려고 hue 주기적으로 업데이트
		// deactivate되면 코루틴이 꺼지게 되므로 OnEnable에서 처리한다
		StartCoroutine(UpdateHue());
	}

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
		if (detailView != null)
		{
			// show 
			var view = GameObject.Instantiate(detailView);
			view.transform.position += new Vector3(0, 0, -5);
		}
    }

	IEnumerator UpdateHue()
	{
		const float shiftRange = 0.04f;
		while(true)
		{
			yield return new WaitForSeconds(0.4f + UnityEngine.Random.value * 0.1f);
			float hueshift = (UnityEngine.Random.value - 0.5f) * shiftRange;

			foreach(SpriteRenderer r in new SpriteRenderer[]{_background, _effect, _symbol})
				r.material.SetFloat("_HueShift", hueshift);
		}
	}
}
