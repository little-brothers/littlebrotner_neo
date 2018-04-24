using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataGame : MonoBehaviour, IMinigame {

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

	public void WrongSound(){
		soundFx.PlayOneShot (wrong);
	}



	[SerializeField]
	public GameObject[] datas;

	private int temp = 0;
	int _score = 0;
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

	public void FalseAnswer() {
		// 감점
		WrongSound();
		_score = Mathf.Max(0, _score - 1);
	}

	public bool CheckAllTrue() {
		ClickSound ();
		for(var i = 0; i<datas.Length; i++){
			if (!datas[i].GetComponent<Data>()._truedata) {
				return false;
			}
		}

		_score += 20;
		ResetAllCards ();
		return true;
	}

	public void ResetAllCards(){

		for(var i = 0; i<datas.Length; i++){
			datas [i].GetComponent<Data> ().toTrue ();
		}

		// initialize
		temp = Random.Range(0, datas.Length);
		datas [temp].GetComponent<Data> ().toFalse ();

		temp = Random.Range(0, datas.Length);
		datas [temp].GetComponent<Data> ().toFalse ();

		temp = Random.Range(0, datas.Length);
		datas [temp].GetComponent<Data> ().toFalse ();
	}

	void IMinigame.Setup()
	{
		ResetAllCards();
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
