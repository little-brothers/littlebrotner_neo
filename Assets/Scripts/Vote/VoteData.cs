using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VoteSelection
{
	NotYet,
	Accept,
	Decline,
	Abstention,
}

public struct VoteData {

	public int id { get; set; }
	public string voteTopic { get; set; }
	public VoteSelection choice { get; set; }
	public int day { get; set; }
	public string eventName { get; set; } // 투표가 활성화되면 발생하는 이벤트 이름
	
	public VoteDetailData agree;
	public VoteDetailData disagree;
	public VoteDetailData abstention;
}

public struct VoteDetailData {
	
	public int nextVoteIndex { get; set; }
	public int economy { get; set; }
	public int political { get; set; }
	public int mechanic { get; set; }
	public Queue<int> endingIndexes;
	public int robotIndex { get; set; }
	public int action; // 다음날이 되기 전에 일어나는 이벤트
}