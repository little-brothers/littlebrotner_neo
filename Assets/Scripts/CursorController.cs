using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour {

//public Camera cam;
	//private float maxWidth;
	public int _point = 0;
	float _end_time;

	const int max_point = 100;
	const float max_time = 10f;


	const float MAX_SCALE = 2f;


	[SerializeField]
	GameObject _time_mask;

	[SerializeField]
	GameObject _point_mask;
	// Use this for initialization


	void Start () {
		_end_time = Time.time + max_time;
		_point_mask.transform.localScale = new Vector3(115 * _point / (float)max_point, 0.3f, 1);
	}

	void UpdateTimeProgress() {
		float leftTime = _end_time - Time.time;
		if (leftTime < 0)
			leftTime = 0f;

		_time_mask.transform.localScale = new Vector3 (115 * leftTime / (float)max_time, 0.3f, 1);
	}

	void Update() {
		UpdateTimeProgress();

		// sync with mouse position
		var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = transform.position.z; // keep distance
		transform.position = mousePos;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		//Debug.Log (temp);

		_point_mask.transform.localScale = new Vector3(115 * _point / (float)max_point, 0.3f, 1);


		if (Input.GetMouseButtonDown (0)) {
			//_point++;
	//		Debug.Log (_point);
	//		Debug.Log (_point / (float)max_point);
					}
	}

	public void PointUp(int value){
		_point += value;
	}

	public void PointDown(int value){
		_point -= value;
	}


	public void CardMatch(int value){

	}

}
