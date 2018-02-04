using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudFx : MonoBehaviour {

	public AudioSource myFx;

	[SerializeField]
	public AudioClip hoverFx;

	[SerializeField]
	public AudioClip clickFx;


	public void HoverSound(AudioClip sound){

		myFx.PlayOneShot (sound);

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
