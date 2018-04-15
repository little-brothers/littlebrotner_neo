using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour {

	public bool DO_NOT = false;


	public GameObject cardgame;


	[SerializeField]
	public int _cardvalue;

	[SerializeField]
	public bool _cardon = false;

	//[SerializeField]
	//public int _cardid;


	[SerializeField]
	public void touchCard(){

		if (DO_NOT) {
		} else {

			Debug.Log ("PUSH CARD:" + _cardvalue);
			if (cardgame.GetComponent<CardGame> ().step == 1) {
				cardgame.GetComponent<CardGame> ().LinkToCard (gameObject);
				cardgame.GetComponent<CardGame> ().Push (_cardvalue);

				this._cardon = true;
				Debug.Log ("AAAA");

			} else if (cardgame.GetComponent<CardGame> ().step == 2 && this._cardon == false) {

				cardgame.GetComponent<CardGame> ().LinkToCard (gameObject);
				this._cardon = true;
				if (cardgame.GetComponent<CardGame> ().Match (_cardvalue)) {
					//cardgame.GetComponent<CardGame> ().LinkToCard (gameObject); // true
				} 


				Debug.Log ("BBBB");
			} else if (cardgame.GetComponent<CardGame> ().step == 2 && this._cardon == true) {

				this._cardon = false;
				cardgame.GetComponent<CardGame> ().StepReset ();
				Debug.Log ("CCCC");
			} else {
				//down point

			}

		}

	}


	public void ImageLoad(Sprite card_image){

		this.GetComponent<Image> ().sprite = card_image;

	}

	void Update(){

		if (this._cardon) {
			GetComponent<Image> ().color = new Color32 (255, 255, 225, 100);
			ImageLoad(cardgame.GetComponent<CardGame>().cardImage[_cardvalue]);
		} else {
			GetComponent<Image> ().color = new Color32 (255, 255, 225, 225);
			ImageLoad(cardgame.GetComponent<CardGame>().cardBack);
		}

	}

	/*
	[SerializeField]
	private int _state;

	[SerializeField]
	private int _cardValue;

	[SerializeField]
	private bool _initialized = false;


	private Sprite _cardBack;
	private Sprite _cardFace;

	private GameObject _manager;


	// Use this for initialization
	void Start () {
		_state = 1;
		_manager = GameObject.FindGameObjectWithTag ("Manager");

	}

	public void setupGraphics(){
		_cardBack = _manager.GetComponent<CardGameManager> ().getCardBack ();
		_cardFace = _manager.GetComponent<CardGameManager> ().getCardFace (_cardValue);

		flipCard ();
	}


	public void flipCard(){

		if (_state == 0)
			_state = 1;
		else if (_state == 1)
			_state = 0;

		if(_state == 0 && !DO_NOT)
			GetComponent<Image> ().sprite = _cardBack;
		else if(_state == 1 && !DO_NOT)
			GetComponent<Image>().sprite = _cardFace;
	}



		public int cardValue {
			get { return _cardValue; }
			set { _cardValue = value; }
		}

		public int state {
			get { return _state; }
			set { _cardValue = value; }
		}

		public bool initialized {
			get { return _initialized; }
			set { _initialized = value; }
		}


		public void falseCheck(){
			StartCoroutine (pause());
		}


		IEnumerator pause() {
			yield return new WaitForSeconds (1);
			if(_state == 0)
				GetComponent<Image>().sprite = _cardBack;
			else if (_state == 1)
				GetComponent<Image>().sprite = _cardFace;
			DO_NOT = false;

		}

	// Update is called once per frame
	void Update () {
		
	}


*/
}
