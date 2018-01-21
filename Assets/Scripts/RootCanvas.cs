using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RootCanvas : MonoBehaviour {

	[SerializeField]
	GameObject[] disableOnPopup;

	void Update() {
		bool on = transform.childCount == 0;

		foreach (var go in disableOnPopup)
			go.SetActive(on);
	}
}
