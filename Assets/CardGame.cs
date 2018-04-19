using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGame : MonoBehaviour, IMinigame {

	[SerializeField]
	public GameObject[] cards;

	[SerializeField]
	public Sprite[] cardImage;

	public Sprite cardBack;

	public int oneside; // store card number
	public int step { get { return _step; } }
	int _step = 0; // 0: wait, 1: one card, 2: two card

	public GameObject one;
	public GameObject two;

	int _score;
	const int MaxScore = 100;
	const float GameTime = 10f;


	List<int> numbers = new List<int>();

	float IMinigame.Progress
	{
		get { return _score / (float)MaxScore; }
	}

	float IMinigame.MaxTime
	{
		get { return GameTime; }
	}


	void IMinigame.Setup()
	{
		_score = 0;
		_step = 1;

		Initialize ();

		for (var i = 0; i < cards.Length; i++) {

			Debug.Log (cards [i].GetComponent<Card> ()._cardvalue);

		}

		StartCoroutine (showing ());

	}

	void IMinigame.Finished()
	{
		_DO_NOT(false);
	}

	bool IMinigame.Tick()
	{
		return false;
	}

	void Initialize(){

		for (int id = 1; id <= 2; id++) {
			for (int d = 0; d < 7; d++) {
				numbers.Add (d);
			}
		}

		int index;

		for(var i = 0; i<cards.Length; i++){
			//Debug.Log ("NUMBER LENGTH:"+numbers.Count);

			index = Random.Range (0, numbers.Count);
			//Debug.Log ("INDEX"+index);
			//Debug.Log ("CARD NUMBER>>>"+numbers[index]);
			cards [i].GetComponent<Card> ()._cardvalue = numbers[index];
			cards [i].GetComponent<Card> ().ImageLoad(cardImage[numbers[index]]);
			numbers.RemoveAt (index);
		}
	}




	public void Push(int cardvalue){
		Debug.Log ("PUSH");
		oneside = cardvalue;
		StepUp ();
	}


	public bool Match(int cardvalue){
		Debug.Log ("MATCH");

		if (cardvalue == oneside) {
			Debug.Log ("!!:" + cardvalue);
			foreach (GameObject c in cards) 
			{
				if (c.GetComponent<Card> ()._cardvalue == cardvalue) {
					Debug.Log ("!" + c.GetComponent<Card> ()._cardvalue);
					c.SetActive (false);
				}
			}

			_score += 15;
			StepReset ();
			return true;

		} else {
			_score -= 2;
			StartCoroutine(pause());

			return false;
		}
	}

	IEnumerator pause(){

		_DO_NOT (true);

			yield return new WaitForSeconds (0.5f);

			one.GetComponent<Card> ()._cardon = false;
			two.GetComponent<Card> ()._cardon = false;

			StepReset ();

		_DO_NOT (false);

	}



	IEnumerator showing(){

		_DO_NOT (true);

		foreach (GameObject c in cards) 
		{
			c.GetComponent<Card> ()._cardon = true;
		}

		yield return new WaitForSeconds (3);

		foreach (GameObject c in cards) 
		{
			c.GetComponent<Card> ()._cardon = false;
		}

		_DO_NOT (false);

	}

	public void StepUp(){
		_step++;
	}

	public void StepReset(){
		oneside = -1;
		_step = 1;
		one = null;
		two = null;
	}

	public void _DO_NOT(bool toggle){

		foreach (GameObject c in cards) 
		{
			c.GetComponent<Card> ().DO_NOT = toggle;
		}

	}


	public void LinkToCard(GameObject temp){


		switch (_step) {

		case 1:
			one = temp;
			Debug.Log ("LINK1:" + temp);
			break;
		case 2:
			two = temp;
			Debug.Log ("LINK2:" + temp);
			break;

		default:
			break;

		}
	}
}
