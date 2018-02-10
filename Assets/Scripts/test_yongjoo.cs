using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class test_yongjoo : MonoBehaviour {

	public enum WatchType {
		Money,
		Tax,
		Health,
	}


	Text _text;
	// Use this for initialization

	[SerializeField]
	WatchType watch;


	void Start () {
		_text = GetComponent<Text>();



		switch (watch) {

		case WatchType.Money:
			int temp = MyStatus.instance.health;
			_text.text = temp.ToString();
			break;


		case WatchType.Health:
			//	int temp = ;
			_text.text = MyStatus.instance.health.ToString();
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
