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

	private EndingData currentEnding {
		get {
			return Database<EndingData>.instance.Find(MyStatus.instance.endingIndex + 1); 
		}
	}
	private StringBuilder _stringBuilder = new StringBuilder();
	private bool _isClick = false;	

	private void Start()
	{
		EndingData ending = currentEnding;
		_questionText.text = ending.endingQuestion;
		EncryptedPlayerPrefs.SetInt("Ending" + ending.id, 1);

		_bgAnimator.setState(ending.id-1);
		StartCoroutine(_bgAnimator.runAnimation());
	}
	
	public void PrintEndingText()
	{
		StartCoroutine(Print(currentEnding.endingDescription + "\n\n" + "Hint: " + currentEnding.endingHint));
	}

	private IEnumerator Print(string sentence)
	{
		_stringBuilder.Remove(0, _stringBuilder.Length);
		StartCoroutine("PrintEncryptText", sentence);
		yield return new WaitForSeconds(_printingSpeed);
		StartCoroutine("ReplaceDecriptText", sentence);		
	}

	private IEnumerator PrintEncryptText(string sentence)
	{
		char[] temp = sentence.ToCharArray(); 

		for (int i = 0; i < temp.Length; ++i)
		{
			_stringBuilder.Append(System.Convert.ToChar(temp[i] + 3));
			_endingText.text = _stringBuilder.ToString();
			yield return new WaitForSeconds(_printingSpeed);
		}
	}

	private IEnumerator ReplaceDecriptText(string sentence)
	{
		char[] temp = sentence.ToCharArray(); 

		for (int i = 0; i < temp.Length; ++i)
		{
			_stringBuilder.Replace(_stringBuilder[i], temp[i], i, 1);
			_endingText.text = _stringBuilder.ToString();
			yield return new WaitForSeconds(_printingSpeed);
		}
	}

	public void ShowEndingTitle()
	{
		//StopAllCoroutines();
		StopCoroutine("Print");
		StopCoroutine("PrintEncryptText");
		StopCoroutine("ReplaceDecriptText");
		StopCoroutine("EndingTitleAnimation");

		if (_isClick)
		{
			SceneManager.LoadScene("CreditScene_Game");
			return;
		}

		_isClick = true;
		StartCoroutine("EndingTitleAnimation");
	}

	private IEnumerator EndingTitleAnimation()
	{
		float timer = 0f;
		while (timer <= 1f)
		{
			timer += Time.deltaTime;
			_endingText.color = Color.Lerp(_endingText.color, new Color(1f, 1f, 1f, 0f), timer);
			yield return null;
		}
		_endingText.text = "";
		_endingText.color = Color.red;
		_endingText.alignment = TextAnchor.UpperLeft;
		StartCoroutine(Print(currentEnding.endingTitle));
	}
}
