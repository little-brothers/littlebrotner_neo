using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour {

//public Camera cam;
	//private float maxWidth;
	private float point = 0f;
	private float time = 0f;

	private float max_point = 100f;
	private float max_time = 10f;


	const float MAX_SCALE = 2f;


	public GameObject _time_mask;
	public GameObject _point_mask;
	// Use this for initialization


	void Start () {
		// _time_mask = transform.Find("TimeMask").gameObject;
		// _point_mask = transform.Find("PointMask").gameObject;
		/*
		if (cam == null) {
			cam = Camera.main;
		}
		Vector3 upperCorner = new Vector3 (Screen.width, Screen.height, 0.0f);
		Vector3 targetWidth = cam.ScreenToWorldPoint (upperCorner);
		maxWidth = targetWidth.x;

*/		time = max_time;
		_time_mask.transform.localScale = new Vector3 (115 * time / max_time, 0.3f, 1);
		_point_mask.transform.localScale = new Vector3(115 * point / max_point, 0.3f, 1);

		}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 temp = Input.mousePosition;
		temp.z = 10f; // Set this to be the distance you want the object to be placed in front of the camera.
		this.transform.position = Camera.main.ScreenToWorldPoint(temp);

		//Debug.Log (temp);

		if (Input.GetMouseButtonDown (0)) {
			point++;
			Debug.Log (point);
			Debug.Log (point / max_point);
			_point_mask.transform.localScale = new Vector3(115 * point / max_point, 0.3f, 1);
		}

		if (time > 0) {
			time -= Time.deltaTime;
			_time_mask.transform.localScale = new Vector3 (115 * time / max_time, 0.3f, 1);
		}

	}


}
