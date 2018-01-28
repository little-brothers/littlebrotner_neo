using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RootCanvas : MonoBehaviour {

	[SerializeField]
	GameObject[] disableOnPopup;

	[SerializeField]
	GameObject[] enableOnPopup;

	void Update() {
		bool on = transform.childCount == 0;

		foreach (var go in disableOnPopup)
			go.SetActive(on);

		foreach (var go in enableOnPopup)
			go.SetActive(!on);
	}

	public void ClearSinglePopup() {
		if (transform.childCount == 0)
			return;

		var child = transform.GetChild(transform.childCount-1);
		GameObject.Destroy(child.gameObject);
	}
}
