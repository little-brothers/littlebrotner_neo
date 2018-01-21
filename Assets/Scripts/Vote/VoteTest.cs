using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoteTest : MonoBehaviour {

	[SerializeField]
	private Text _day;

	[SerializeField]
	private Text _topic;

	[SerializeField]
	private Text _index;

	void Start () {
		VoteManager.Initialize("vote");
		ShowVote();
	}
	
	public void ShowVote()
	{
		VoteData data = VoteManager.currentVote;
		_day.text = data.day.ToString();
		_topic.text = data.voteTopic;
	}

	public void NextDay()
	{
		VoteManager.NextDay();
		ShowVote();
	}

	public void Yes()
	{
		VoteManager.Vote(1);
	}

	public void No()
	{
		VoteManager.Vote(0);
	}
}
