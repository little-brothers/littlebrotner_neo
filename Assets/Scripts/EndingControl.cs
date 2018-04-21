using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingControl : MonoBehaviour {

	[SerializeField]
	private SpriteAnimator _bgAnimator;

	[SerializeField]
	private Text _questionText;

	[SerializeField]
	private Text _endingText;

	[SerializeField]
	private float _printingSpeed;
	[SerializeField]
	private float _blinkSpeed;

	private EndingData currentEnding {
		get {
			return Database<EndingData>.instance.Find(MyStatus.instance.endingIndex + 1); 
		}
	}
	private bool _isClick = false;	

	private void Start()
	{
		EndingData ending = currentEnding;
		_questionText.text = ending.endingQuestion;
		EncryptedPlayerPrefs.SetInt("Ending" + ending.id, 1);

		_bgAnimator.setState(ending.id-1);
		StartCoroutine(_bgAnimator.runAnimation());
		GameObject.Find("Soundmanager").GetComponent<Soundmanager> ().EndingPlay ();
	}
	
	public void PrintEndingText()
	{
		StartCoroutine(EndingSequence());
	}

	IEnumerator EndingSequence()
	{
		// print ending description
		var descPrint = Print(currentEnding.endingDescription + "\n\n" + "Hint: " + currentEnding.endingHint);
		while (descPrint.MoveNext()) {
			yield return descPrint.Current;
		}

		// wait for click
		var waitForClick = WaitForClick();
		while (waitForClick.MoveNext()) {
			yield return waitForClick.Current;
		}

		// fade out
		float timer = 0f;
		while (timer <= 1f)
		{
			timer += Time.deltaTime;
			_endingText.color = Color.Lerp(_endingText.color, new Color(1f, 1f, 1f, 0f), timer);
			yield return null;
		}
		_endingText.text = "";
		_endingText.color = new Color(231/225f, 52/225f, 57/225f);
		_endingText.alignment = TextAnchor.UpperLeft;

		//
		IEnumerator namePrint = Print(currentEnding.id +"_ "+ currentEnding.endingTitle);
		while (namePrint.MoveNext()) {
			yield return namePrint.Current;
		}

		// wait for click
		waitForClick = WaitForClick();
		while (waitForClick.MoveNext()) {
			yield return waitForClick.Current;
		}

		// next scene
		SceneManager.LoadScene("CreditScene_Game");
	}

	private IEnumerator Print(string sentence)
	{
		StringBuilder progress = new StringBuilder();
		char[] characters = sentence.ToCharArray();

		for (int i = 0; i < characters.Length; ++i)
		{
			// last character encrypted
			progress.Append(Convert.ToChar(characters[i] + 3));

			// decrypt last character
			if (i > 0) {
				progress[i-1] = characters[i-1];
				_endingText.text = progress.ToString();
			}

			yield return new WaitForSeconds(_printingSpeed);
		}

		// full text
		_endingText.text = sentence;
	}

	IEnumerator WaitForClick() {
		_isClick = false;
		string text = _endingText.text;
		while (!_isClick) {
			yield return new WaitForSeconds(_blinkSpeed);
			_endingText.text = text + "_";

			// fast escape
			if (_isClick) break;

			yield return new WaitForSeconds(_blinkSpeed);
			_endingText.text = text;
		}

		_endingText.text = text;
	}

	public void ShowEndingTitle()
	{
		_isClick = true;
	}
}
