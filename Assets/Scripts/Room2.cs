using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class Room2 : MonoBehaviour {

	void OnEnable() {
		// turn post processing on
		Camera.main.GetComponent<PostProcessingBehaviour>().enabled = true;
	}

	void OnDisable() {
		// turn post processing off
		if (Camera.main != null) {
			Camera.main.GetComponent<PostProcessingBehaviour>().enabled = false;
		}
	}
}
