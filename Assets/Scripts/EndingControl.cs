using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public struct EndingData
{
	public string endingQuestion { set; get; }
	public string endingTitle { set; get; }
	public string endingDescription { set; get; }
	public string endingHint {set; get; }
}

public class EndingControl : MonoBehaviour {

	[SerializeField]
	private SpriteAnimator _bgAnimator;

	[SerializeField]
	private Text _questionText;

	[SerializeField]
	private Text _endingText;

	[SerializeField]
	private float _printingSpeed;

	private List<EndingData> _endingDatas;
	private StringBuilder _stringBuilder;
	private int _currentIndex = 0;
	private bool _isClick = false;	

	private void Awake () 
	{
		LoadEndingData();
		_stringBuilder = new StringBuilder();
		_currentIndex = MyStatus.instance.endingIndex;
		_questionText.text = _endingDatas[_currentIndex].endingQuestion;
		EncryptedPlayerPrefs.SetInt("Ending" + (_currentIndex + 1), 1);
	}
	
	private void Start()
	{
		_bgAnimator.setState(_currentIndex);
		StartCoroutine(_bgAnimator.runAnimation());
	}
	
	private void LoadEndingData()
	{
		_endingDatas = new List<EndingData>();
		TextAsset csv = Resources.Load("ending") as TextAsset;
		using (var streamReader = new StreamReader(new MemoryStream(csv.bytes)))
		{
			using (var reader = new Mono.Csv.CsvFileReader(streamReader))
			{
				var stringArray = new List<List<string>>();
				reader.ReadAll(stringArray);
				stringArray.RemoveAt(0);

				//Index 0: 현재 투표 인덱스
				//Index 1: 날짜
				//Index 2: 질문
				//Index 4: 제목
				//Index 9: 설명
				//Index 12: 힌트
				foreach(List<string> column in stringArray)
				{
					EndingData data = new EndingData();
					data.endingQuestion = column[2];
					data.endingHint = column[12];
					data.endingDescription = column[9];
					data.endingTitle = column[4];
					_endingDatas.Add(data);
				}
			}
		}
	}

	public void PrintEndingText()
	{
		StartCoroutine("Print", (_endingDatas[_currentIndex].endingDescription + "\n\n" + "Hint: " + _endingDatas[_currentIndex].endingHint));
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
		StartCoroutine("Print", _endingDatas[_currentIndex].endingTitle);
	}
}
