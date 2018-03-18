using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct Vote
{
	public Vote(int id)
	{
		this.id = id;
		this.selection = VoteSelection.NotYet;
	}

	public int id;
	public VoteSelection selection;
}

public static class VoteManager {
	public static Vote currentVote { get { return _current; } }

	public static int abstentionCount { get { return _abstentionCount; } }
	public static List<Vote> history { get { return _history; } }

	private static Vote _current;
	private static int _abstentionCount = 0;
	private static List<Vote> _history = new List<Vote>();

	public static void Initialize()
	{
		_current = new Vote(1);
	}

	public static void NextDay()
	{
		if (_current.selection == VoteSelection.NotYet)
			Vote(VoteSelection.Abstention); // 자동 기권

		var voteData = Database<VoteData>.instance.Find(_current.id);
		var result = voteData.agree;
		string state = "예";
		if (_current.selection == VoteSelection.Abstention)
		{
			state = "기권";
			result = voteData.abstention;
		}
		else if (_current.selection == VoteSelection.Decline)
		{
			state = "아니오";
			result = voteData.disagree;
		}

		if (_current.selection == VoteSelection.Abstention)
			_abstentionCount++;
		else
			_abstentionCount = 0;

		MyStatus.instance.economy.value += result.economy;
		MyStatus.instance.political.value += result.political;
		MyStatus.instance.mechanic.value += result.mechanic;

		// add to history
		_history.Add(_current);

		// next vote
		_current = new Vote(result.nextVote);

		Debug.Log("이전 투표 결과: " + state + " | BI: " + _history.Last().id + " | CI: " + _current.id);
	}

	public static void Vote(VoteSelection choice)
	{
		_current.selection = choice;
	}
}