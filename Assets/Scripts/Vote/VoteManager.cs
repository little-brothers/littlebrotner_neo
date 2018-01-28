using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class VoteManager {

	private static List<VoteData> _voteDatas;
	public static List<VoteData> voteDatas { get { return _voteDatas; } }
	public static VoteData currentVote {get { return _voteDatas[_currentIndex]; } }

	private static List<string> _endingCodition;

	private static int _currentIndex = 0;
	private static int _beforIndex = 0;
	private static int _abstentionCount = 0;

	public static bool Initialize(string fileName)
	{
		_voteDatas = new List<VoteData>();
		_endingCodition = new List<string>();
		
		TextAsset csv = Resources.Load(fileName) as TextAsset;
		if (csv == null)
			return false;

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
				//Index 7: 동의시 다음 투표 인덱스
				//Index 8: 비동의시 다음 투표 인덱스
				//Index 9 : 기권시 다음 투표 값 (에 || 아니오)
				//Index 10: 동의 economy
				//Index 11: 동의 political
				//Index 12: 동의 mechanic
				//Index 13: 비동의 economy
				//Index 14: 비동의 political
				//Index 15: 비동의 mechanic
				//Index 19: 엔딩조건
				foreach(List<string> column in stringArray)
				{
					if (!column[19].Equals(""))
						_endingCodition.Add(column[19]);

					_voteDatas.Add(GenerateVoteData(column));
				}
			}
		}
		return true;
	}

	private static void SetNextVoteIndex(string str, ref VoteDetailData data)
	{
		string[] nextIndexes = str.Split('/');
		data.endingIndexes = new Queue<int>();

		foreach (string nextIndex in nextIndexes)
		{
			if (nextIndex.Contains("E"))
			{
				int endingIndex = Int32.Parse(nextIndex.Substring(1)) - 1;
				data.endingIndexes.Enqueue(endingIndex);
			}
			else if (nextIndex.Contains("V-R"))
			{
				data.nextVoteIndex = Int32.Parse(nextIndex.Substring(3)) - 1;
			}
			else
			{
				data.nextVoteIndex = Int32.Parse(nextIndex.Substring(1)) - 1;
				//Debug.Log(data.nextVoteIndex);
			}
		}
	}

	private static VoteData GenerateVoteData(List<string> column)
	{
		VoteData data = new VoteData();
		data.day = Int32.Parse(column[1]);
		data.voteTopic = column[2];
		data.isAgree = -1;

		VoteDetailData agree = new VoteDetailData();
		SetNextVoteIndex(column[7], ref agree);
		agree.economy = column[10].Equals("") ? 0 : Int32.Parse(column[10]);
		agree.political = column[11].Equals("") ? 0 : Int32.Parse(column[11]);
		agree.mechanic = column[12].Equals("") ? 0 : Int32.Parse(column[12]);

		VoteDetailData disagree = new VoteDetailData();
		SetNextVoteIndex(column[8], ref disagree);
		disagree.economy = column[13].Equals("") ? 0 : Int32.Parse(column[13]);
		disagree.political = column[14].Equals("") ? 0 : Int32.Parse(column[14]);
		disagree.mechanic = column[15].Equals("") ? 0 : Int32.Parse(column[15]);

		VoteDetailData abstention = new VoteDetailData();
		bool peoplesChoice = column[9].CompareTo("예").Equals(0) ? true : false;
		if (peoplesChoice)
		{
			abstention.nextVoteIndex = agree.nextVoteIndex;
			abstention.economy = agree.economy;
			abstention.political = agree.political;
			abstention.mechanic = agree.mechanic;
		}
		else
		{
			abstention.nextVoteIndex = disagree.nextVoteIndex;
			abstention.economy = disagree.economy;
			abstention.political = disagree.political;
			abstention.mechanic = disagree.mechanic;
		}

		data.agree = agree;
		data.disagree = disagree;
		data.abstention = abstention;

		return data;
	}

	public static void NextDay()
	{
		_beforIndex = _currentIndex;				

		string state = "?";
		if (_voteDatas[_currentIndex].isAgree.Equals(-1))
		{
			state = "기권";
			_currentIndex = _voteDatas[_currentIndex].abstention.nextVoteIndex;
			MyStatus.instance.economy.value += _voteDatas[_currentIndex].abstention.economy;
			MyStatus.instance.political.value += _voteDatas[_currentIndex].abstention.political;
			MyStatus.instance.mechanic.value += _voteDatas[_currentIndex].abstention.mechanic;
			++_abstentionCount;
			CheckEnding(_voteDatas[_beforIndex].abstention);
		}
		else if (_voteDatas[_currentIndex].isAgree.Equals(1))
		{
			state = "예";
			_currentIndex = _voteDatas[_currentIndex].agree.nextVoteIndex;
			MyStatus.instance.economy.value += _voteDatas[_currentIndex].agree.economy;
			MyStatus.instance.political.value += _voteDatas[_currentIndex].agree.political;
			MyStatus.instance.mechanic.value += _voteDatas[_currentIndex].agree.mechanic;
			CheckEnding(_voteDatas[_beforIndex].agree);
		}
		else if (_voteDatas[_currentIndex].isAgree.Equals(0))
		{
			state = "아니오";
			_currentIndex = _voteDatas[_currentIndex].disagree.nextVoteIndex;
			MyStatus.instance.economy.value += _voteDatas[_currentIndex].disagree.economy;
			MyStatus.instance.political.value += _voteDatas[_currentIndex].disagree.political;
			MyStatus.instance.mechanic.value += _voteDatas[_currentIndex].disagree.mechanic;
			CheckEnding(_voteDatas[_beforIndex].disagree);
		}


		Debug.Log("이전 투표 결과: " + state + " | BI: " + _beforIndex + " | CI: " + _currentIndex);
	}

	public static void Vote(int agree)
	{
		VoteData temp = _voteDatas[_currentIndex];
		temp.isAgree = agree;
		_voteDatas[_currentIndex] = temp;
	}

	private static void CheckEnding(VoteDetailData data)
	{
		if (data.endingIndexes == null)
		{
			if (MyStatus.instance.health.value.Equals(0)) 
			{
				MyStatus.instance.endingIndex.value = 0;
				SceneManager.LoadScene("EndingScene");
			}
			else if (_abstentionCount.Equals(3))
			{
				MyStatus.instance.endingIndex.value = 1;
				SceneManager.LoadScene("EndingScene");
			}
			else if (MyStatus.instance.money.value >= 30)
			{
				MyStatus.instance.endingIndex.value = 19;
				SceneManager.LoadScene("EndingScene");
			}
			return;
		}

		foreach (int index in data.endingIndexes)
		{
			string codition = _endingCodition[index];
			if (IsEnding(codition))
			{
				MyStatus.instance.endingIndex.value = index;
				SceneManager.LoadScene("EndingScene");
			}
		}
	}

	private static bool IsEnding(string codition)
	{
		string[] coditions = codition.Split('&');
		bool returnValue = CheckCodition(coditions[0]);

		if (coditions.Length.Equals(1))
			return returnValue;

		for (int i = 1; i < coditions.Length; ++i)
		{
			if (!returnValue)
				break;
			returnValue = returnValue && CheckCodition(coditions[i]);
		}
		return returnValue;
	}

	private static bool CheckCodition(string codition)
	{
		bool returnValue = false;
		string[] value = codition.Split('-');
		switch(value[0])
		{
			// case "Health": // 투표 결과에 영향을 받지 않는 엔딩은 바로 수행
			// 	if (MyStatus.instance.health.value.Equals(Int32.Parse(value[1]))) 
			// 	{
			// 		MyStatus.instance.endingIndex.value = 0;
			// 		SceneManager.LoadScene("EndingScene");
			// 	}
			// 	break;

			// case "Abstention": // 투표 결과에 영향을 받지 않는 엔딩은 바로 수행
			// 	if (_abstentionCount.Equals(Int32.Parse(value[1])))
			// 	{
			// 		MyStatus.instance.endingIndex.value = 1;
			// 		SceneManager.LoadScene("EndingScene");
			// 	}
			// 	break;

			// case "Money": // 투표 결과에 영향을 받지 않는 엔딩은 바로 수행
			// 	if (MyStatus.instance.money.value.Equals(Int32.Parse(value[1])))
			// 	{
			// 		MyStatus.instance.endingIndex.value = 19;
			// 		SceneManager.LoadScene("EndingScene");
			// 	}
			// 	break;

			case "자유주의": // 나중에 자유주의, 사회주의등 가장 높은 수치 반환하는 함수 만들기
				break;

			case "사회주의":
				break;

			default:
				int index = Int32.Parse(value[0].Substring(1)) - 1;
				if (value[1].Equals("YES"))
				{
					returnValue = _voteDatas[index].agree.Equals(1) ? true : false;
				}
				else if (value[1].Equals("NO"))
				{
					returnValue = _voteDatas[index].agree.Equals(0) ? true : false;
				}
				break;
		}
		return returnValue;
	}
}