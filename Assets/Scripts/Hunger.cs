using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunger : MonoBehaviour {

	public GameObject _1;
	public GameObject _2;
	public GameObject _3;
/*
	[SerializeField]
	Color SickColor = Color.red;
*/

	// Use this for initialization
	void Start () {
		//_mask = transform.Find("GaugeMask").gameObject;

		// auto update gauge
		MyStatus.instance.hunger.OnUpdate += updateGauge;
		updateGauge(MyStatus.instance.hunger);

		// initial color
	}

	void OnDestroy() {
		MyStatus.instance.hunger.OnUpdate -= updateGauge;
	}

	void updateGauge(int value) {

			_1.SetActive (value>=1);
			_2.SetActive (value>=2);
			_3.SetActive (value>=3);

	}

}
