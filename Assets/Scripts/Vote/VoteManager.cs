using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class VoteManager {

	private static List<VoteData> _voteDatas;
	public static List<VoteData> voteDatas { get { return _voteDatas; } }
	public static VoteData currentVote {get { return _voteDatas[_currentIndex]; } }
	public static int abstentionCount { get { return _abstentionCount; } }

	private static int _currentIndex = 0;
	private static int _beforIndex = 0;
	private static int _abstentionCount = 0;

	public static bool Initialize(string fileName)
	{
		_voteDatas = new List<VoteData>();
		
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
				//Index 16: 동의 이벤트
				//Index 17: 거부 이벤트
				//Index 19: 활성화 이벤트
				foreach(List<string> column in stringArray)
				{
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
		data.id = Int32.Parse(column[0].Substring(1));
		data.day = Int32.Parse(column[1]);
		data.voteTopic = column[2];
		data.choice = VoteSelection.NotYet;
		data.eventName = column[19];

		VoteDetailData agree = new VoteDetailData();
		SetNextVoteIndex(column[7], ref agree);
		agree.economy = column[10].Equals("") ? 0 : Int32.Parse(column[10]);
		agree.political = column[11].Equals("") ? 0 : Int32.Parse(column[11]);
		agree.mechanic = column[12].Equals("") ? 0 : Int32.Parse(column[12]);
		agree.action = column[16].StartsWith("E") ? Int32.Parse(column[16].Substring(1, column[16].IndexOf(":")-1)) : 0;

		VoteDetailData disagree = new VoteDetailData();
		SetNextVoteIndex(column[8], ref disagree);
		disagree.economy = column[13].Equals("") ? 0 : Int32.Parse(column[13]);
		disagree.political = column[14].Equals("") ? 0 : Int32.Parse(column[14]);
		disagree.mechanic = column[15].Equals("") ? 0 : Int32.Parse(column[15]);
		disagree.action = column[17].StartsWith("E") ? Int32.Parse(column[17].Substring(1, column[17].IndexOf(":")-1)) : 0;

		VoteDetailData abstention = disagree;
		bool peoplesChoice = column[9].CompareTo("예").Equals(0) ? true : false;
		if (peoplesChoice)
			abstention = agree;

		data.agree = agree;
		data.disagree = disagree;
		data.abstention = abstention;

		return data;
	}

	public static void NextDay()
	{
		_beforIndex = _currentIndex;				

		string state = "?";
		if (_voteDatas[_currentIndex].choice == VoteSelection.NotYet)
		{
			Vote(VoteSelection.Abstention); // 자동 기권

			state = "기권";
			_currentIndex = _voteDatas[_currentIndex].abstention.nextVoteIndex;
			MyStatus.instance.economy.value += _voteDatas[_currentIndex].abstention.economy;
			MyStatus.instance.political.value += _voteDatas[_currentIndex].abstention.political;
			MyStatus.instance.mechanic.value += _voteDatas[_currentIndex].abstention.mechanic;
			++_abstentionCount;
		}
		else if (_voteDatas[_currentIndex].choice == VoteSelection.Accept)
		{
			state = "예";
			_currentIndex = _voteDatas[_currentIndex].agree.nextVoteIndex;
			MyStatus.instance.economy.value += _voteDatas[_currentIndex].agree.economy;
			MyStatus.instance.political.value += _voteDatas[_currentIndex].agree.political;
			MyStatus.instance.mechanic.value += _voteDatas[_currentIndex].agree.mechanic;
		}
		else if (_voteDatas[_currentIndex].choice == VoteSelection.Decline)
		{
			state = "아니오";
			_currentIndex = _voteDatas[_currentIndex].disagree.nextVoteIndex;
			MyStatus.instance.economy.value += _voteDatas[_currentIndex].disagree.economy;
			MyStatus.instance.political.value += _voteDatas[_currentIndex].disagree.political;
			MyStatus.instance.mechanic.value += _voteDatas[_currentIndex].disagree.mechanic;
		}


		Debug.Log("이전 투표 결과: " + state + " | BI: " + _beforIndex + " | CI: " + _currentIndex);
	}

	public static void Vote(VoteSelection choice)
	{
		VoteData temp = _voteDatas[_currentIndex];
		temp.choice = choice;
		_voteDatas[_currentIndex] = temp;
	}
}