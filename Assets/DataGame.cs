using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataGame : MonoBehaviour {


	public GameObject cursor;

	[SerializeField]
	public GameObject[] datas;
	private int temp = 0;
		
	// Use this for initialization
	void Start () {

		ResetAllCards ();
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void FalseAnswer(){


		cursor.GetComponent<CursorController> ().PointDown (1);

	}



	public bool CheckAllTrue(){


		for(var i = 0; i<datas.Length; i++){
			if (datas[i].GetComponent<Data> ()._truedata != true) {
				return false;
			}
		}
		ResetAllCards ();
		cursor.GetComponent<CursorController> ().PointUp (9);
		return true;
	}

	public void ResetAllCards(){

		for(var i = 0; i<datas.Length; i++){
			datas [i].GetComponent<Data> ().toTrue ();
		}

		Initialize ();
	}


	public void Initialize(){

		temp = Random.Range(0, datas.Length);
		datas [temp].GetComponent<Data> ().toFalse ();

		temp = Random.Range(0, datas.Length);
		datas [temp].GetComponent<Data> ().toFalse ();

		temp = Random.Range(0, datas.Length);
		datas [temp].GetComponent<Data> ().toFalse ();


	}

}
