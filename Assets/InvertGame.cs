using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertGame : MonoBehaviour {


	public GameObject cursor;


	[SerializeField]
	public GameObject[] images;

	[SerializeField]
	public GameObject[] buttons;


	List<bool> password = new List<bool>();
	//List<bool> answer = new List<bool>();


	// Use this for initialization
	void Start () {

		SettingChild ();
		Initialize ();
	}


	void Initialize(){
		SettingPassword ();


		for(int i=0; i<16; i++) {
			images[i].SetActive (password [i]);
		}

		SettingButton();
	}

	void SettingPassword(){

		for(int i=0; i<16; i++) {
			if (Random.value > 0.5)
				password.Add (true);
			else
				password.Add(false);
		}

	}


	void SettingChild(){
		for(int i=1; i<16; i++) {
			images[i] = GameObject.Find("Image (" + i.ToString()+")"); 
			buttons[i] = GameObject.Find("Button (" + i.ToString()+")"); 
			}

	}





	void SettingButton(){

		for(int i=0; i<16; i++) {
			if (Random.value > 0.5)
				buttons [i].GetComponent<Invert> ().status_view = true;
			else
				buttons [i].GetComponent<Invert> ().status_view = false;
		}

	}



	public bool ResultCheck(){
		for (int i = 0; i < 16; i++) {
			if (password [i] != buttons [i].GetComponent<Invert> ().status_view) {
				return false;
			}

		}

		// cursor.GetComponent<CursorController> ().PointUp (50);
		password.Clear ();
		Initialize ();
		return true;
	}


	// Update is called once per frame
	void Update () {
		
	
	}
}
