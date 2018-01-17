using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoteDetailView : MonoBehaviour {


	// Use this for initialization
	void Start () {
		if (GetComponent<Canvas>() != null)
		{
			// ui 컴포넌트인 경우 캔버스로 옮겨준다
			var uiRoot = GameObject.FindGameObjectWithTag("RootCanvas");
			Utilities.SetUIParentFit(uiRoot, gameObject);
		}
	}

	public void OnAnswer(bool accept)
	{
		Debug.Log(accept.ToString());
		GameObject.Destroy(gameObject);
	}

	void Update () {
		
	}
}
