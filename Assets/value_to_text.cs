using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class value_to_text : MonoBehaviour {


	public enum DataType {
		Economy,
		Political,
		Science,
		Money,
		Worked,
		Day
	}


	Text _text;
	// Use this for initialization

	[SerializeField]
	DataType watch;


	void Start () {
		_text = GetComponent<Text>();

		Debug.Log (MyStatus.instance);
		Debug.Log (MyStatus.instance.energy);


			switch (watch) {

		case DataType.Money:
			_text.text = MyStatus.instance.money.value.ToString();
			break;
		

		case DataType.Worked:
		//	int temp = ;
			_text.text = MyStatus.instance.health.value.ToString();
			break;
		

		case DataType.Day:
		//	int temp = ;
			_text.text = MyStatus.instance.day.value.ToString();
			break;



		case DataType.Economy:
			//	int temp = ;
			_text.text = MyStatus.instance.economy.value.ToString();
			break;

		case DataType.Political:
			//	int temp = ;
			_text.text = MyStatus.instance.political.value.ToString();
			break;

		case DataType.Science:
			//	int temp = ;
			_text.text = MyStatus.instance.mechanic.value.ToString();
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



