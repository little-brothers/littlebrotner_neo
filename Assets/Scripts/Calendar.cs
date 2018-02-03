using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calendar : MonoBehaviour {

	TextMesh _text;
	// Use this for initialization
	void Start () {
		_text = GetComponent<TextMesh>();

		MyStatus.instance.day.OnUpdate += UpdateDate;
		UpdateDate(MyStatus.instance.day);
	}

	void OnDestroy() {
		MyStatus.instance.day.OnUpdate -= UpdateDate;
	}

	void UpdateDate(int day)
	{
		_text.text = day.ToString("00");
	}
}
