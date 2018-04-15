using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Data : MonoBehaviour {


	public GameObject datagame;

	public Sprite _false;
	public Sprite _true;


	[SerializeField]
	public bool _truedata;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}



	public void touchData(){

		if (_truedata == false) {

			toTrue ();
			datagame.GetComponent<DataGame> ().CheckAllTrue ();

		} else {

			// minus point
			datagame.GetComponent<DataGame> ().FalseAnswer ();
			// GameObject.Destroy (gameObject);


		}
	}

	public void toTrue(){

		_truedata = true;
		this.GetComponent<Image> ().sprite = _true;

	}

	public void toFalse(){

		_truedata = false;
		this.GetComponent<Image> ().sprite = _false;

	}

}
