using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthGauge : MonoBehaviour {

	const float MAX_SCALE = 2f;

	GameObject _mask;

	// Use this for initialization
	void Start () {
		_mask = transform.Find("Mask").gameObject;
		MyStatus.instance.health.OnUpdate += updateGauge;
		updateGauge(MyStatus.instance.health);
	}

	void updateGauge(int value) {
		_mask.transform.localScale = new Vector3(MAX_SCALE * value / MyStatus.MaxHealth, 1, 1);
	}
}
