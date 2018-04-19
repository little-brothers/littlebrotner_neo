using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour {
	void Update() {
		// sync with mouse position
		var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = transform.position.z; // keep distance
		transform.position = mousePos;
	}
}
