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
		VoteManager.Initialize();
		ShowVote();
	}
	
	// 투표 내용을 띄워줌
	public void ShowVote()
	{
		VoteData data = Database<VoteData>.instance.Find(VoteManager.currentVote.id);
		_day.text = data.day.ToString();
		_topic.text = data.voteTopic;
	}

	// 다음날
	public void NextDay()
	{
		VoteManager.NextDay();
		ShowVote();
	}

	// 투표 동의
	public void Yes()
	{
		VoteManager.Vote(VoteSelection.Accept);
	}

	// 투표 반대
	public void No()
	{
		VoteManager.Vote(VoteSelection.Decline);
	}
}
