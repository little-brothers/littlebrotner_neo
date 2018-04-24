using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Height : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler {



	public GameObject heightgame;


	[SerializeField]
	public int level;


	public void OnPointerDown(PointerEventData data)
	{
		int temp = gameObject.GetComponent<Height> ().level;
		if (heightgame.GetComponent<HeigthGame> ().Matching (temp) == 1) {
			gameObject.SetActive (false);
			heightgame.GetComponent<HeigthGame> ().PointUp ();
		}
		//Debug.Log ("WHY");
		//invertgame.GetComponent<InvertGame> ().ResultCheck ();
	}

	public void OnPointerEnter(PointerEventData data)
	{
		//Debug.Log("MouseOver");
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void Resize(){

		float temp = 0.0003f * level;
		transform.localScale = new Vector3 (0.2f, 0.0004f+temp, 1);

	}
}
