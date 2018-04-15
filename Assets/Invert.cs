using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Invert : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler {

	public GameObject invertgame;



	// 인터페이스 트리거 관련
	public void OnPointerDown(PointerEventData data)
	{
		gameObject.GetComponent<Invert> ().status_view = !gameObject.GetComponent<Invert> ().status_view;
		//Debug.Log ("WHY");
		invertgame.GetComponent<InvertGame> ().ResultCheck ();
	}

	public void OnPointerEnter(PointerEventData data)
	{
		//Debug.Log("MouseOver");
	}
		

	[SerializeField]
	public bool status_view = true;

	// Use this for initialization
	void Start () {


	}
		

	// Update is called once per frame
	void Update () {

		if (status_view) {
			GetComponent<Image> ().color = new Color32 (255, 255, 225, 255);

		} else {
			GetComponent<Image> ().color = new Color32 (255, 255, 225, 1);
			//ImageLoad(cardgame.GetComponent<CardGame>().cardBack);
		}


	}



	public void TouchInvert(){

		//gameObject.GetComponent<Invert>().status_view = !(gameObject.GetComponent<Invert>().status_view);
}


}
