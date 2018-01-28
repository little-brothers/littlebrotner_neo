using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct VoteData {

	public int id { get; set; }
	public string voteTopic { get; set; }
	public int isAgree { get; set; }
	public int day { get; set; }
	
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
}