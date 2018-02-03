using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthGauge : MonoBehaviour {

	const float MAX_SCALE = 2f;

	GameObject _mask;

	[SerializeField]
	Color HealthyColor = Color.green;

	[SerializeField]
	Color SickColor = Color.red;

	// Use this for initialization
	void Start () {
		_mask = transform.Find("GaugeMask").gameObject;

		// auto update gauge
		MyStatus.instance.health.OnUpdate += updateGauge;
		updateGauge(MyStatus.instance.health);

		// initial color
		MyStatus.instance.sick.OnUpdate += updateDisease;
		updateDisease(MyStatus.instance.sick);
	}

	void OnDestroy() {
		MyStatus.instance.health.OnUpdate -= updateGauge;
	}

	void updateGauge(int value) {
		_mask.transform.localScale = new Vector3(1, MAX_SCALE * value / MyStatus.MaxHealth, 1);
	}

	void updateDisease(bool sick) {
		Color healthColor = sick ? SickColor : HealthyColor;
		var renderers = GetComponentsInChildren<SpriteRenderer>();
		foreach (var renderer in renderers) {
			renderer.color = healthColor;
		}
	}
}
