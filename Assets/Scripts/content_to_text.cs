using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class content_to_text : MonoBehaviour {


	public enum DataType {
		Economy,
		Political,
	}


	Text _text;
	// Use this for initialization

	[SerializeField]
	DataType watch;


	void Start () {
		_text = GetComponent<Text>();

		switch (watch) {



		case DataType.Economy:
			//	int temp = ;
			if (MyStatus.instance.economy.value >= 0) {
				_text.text = "socialism";
				_text.color = new Color(255/225f, 231/225f, 23/225f);
			} else {
				_text.text = "capitalism";
			}
			break;

		case DataType.Political:
	
			if (MyStatus.instance.political.value >= 0) {
				_text.text = "Liberalism";

			} else {
				_text.text = "totalitarianism";
				_text.color = new Color(231/225f, 52/225f, 57/225f);
			}
			break;
		

		}


		if (_text == null) {
			Debug.Assert(false, "need valid notifier type!");
			return;
		}


	}

	// Update is called once per frame
	void Update () {

	}

}