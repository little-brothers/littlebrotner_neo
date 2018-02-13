using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnFx : MonoBehaviour {

	public AudioSource myFx;
	public AudioClip hoverFx;
	public AudioClip clickFx;


	public void HoverSound(){

		myFx.PlayOneShot (hoverFx);

	}

	public void ClickSound(){

		myFx.PlayOneShot (clickFx);

	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
