using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertGame : MonoBehaviour, IMinigame {

	[SerializeField]
	AudioSource soundFx;

	[SerializeField]
	AudioClip click;

	[SerializeField]
	AudioClip hover;

	[SerializeField]
	AudioClip wrong;



	public void HoverSound(){
		soundFx.PlayOneShot (hover);
	}

	public void ClickSound(){
		soundFx.PlayOneShot (click);
	}



	[SerializeField]
	public GameObject[] images;

	[SerializeField]
	public GameObject[] buttons;




	List<bool> password = new List<bool>();
	int _score;
	const int MaxScore = 100;
	const float GameTime = 10f;

	float IMinigame.Progress
	{
		get { return _score / (float)MaxScore; }
	}

	float IMinigame.MaxTime
	{
		get { return GameTime; }
	}

	// Use this for initialization
	void Awake() {
		SettingChild ();
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

		ClickSound ();


		for (int i = 0; i < 16; i++) {
			if (password [i] != buttons [i].GetComponent<Invert> ().status_view) {
				return false;
			}

		}

		_score += 50;

		password.Clear ();
		Initialize ();
		return true;
	}


	void IMinigame.Setup()
	{
		Initialize();
	}

	void IMinigame.Finished()
	{
		// throw new System.NotImplementedException();
	}

	bool IMinigame.Tick()
	{
		return false;
	}
}
