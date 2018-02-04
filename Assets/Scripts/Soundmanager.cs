using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soundmanager : MonoBehaviour {

	public AudioSource efxSource;
	public AudioSource MenuMusic;
	public AudioSource IntroMusic;
	public AudioSource MainMusic;
	public AudioSource EndingMusic;

	public static Soundmanager instance = null;

	public float lowPitchRange = .95f;
	public float highPitchRange = 1.05f;

	void Awake(){

		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);

		MenuMusic.loop = true;
		MenuMusic.Play ();


	}

	public void IntroPlay (){

		MenuMusic.Stop ();
		MenuMusic.loop = false;
		IntroMusic.loop = true;
		IntroMusic.Play ();

		Debug.Log ("INTRO");

	}


	public void MainPlay (){

		IntroMusic.Stop ();
		MainMusic.loop = true;
		MainMusic.Play ();

	}

	public void EndingPlay (){

		MainMusic.Stop ();
		EndingMusic.loop = true;
		EndingMusic.Play ();

	}


	public void PlaySingle(AudioClip clip){


		efxSource.clip = clip;
		efxSource.Play ();

	}


	public void RandomizeSfx (params AudioClip [] clips){

		int randomIndex = Random.Range (0, clips.Length);
		float randomPitch = Random.Range (lowPitchRange, highPitchRange);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
