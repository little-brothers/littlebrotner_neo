using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectClick : MonoBehaviour {


	void OnMouseDown()
	{
		Debug.Log ("CLick");
		Destroy (gameObject);
	}
	// Use this for initialization


	void Start () {


		
	}


	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButton(0))
			Debug.Log("Mouse down");
		
	}
}
