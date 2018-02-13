using System;
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

[AssetPath("vote")]
public struct VoteData : IDatabaseRow {

	public int id { get; set; }
	public string voteTopic { get; set; }
	public int day { get; set; }
	public string eventName { get; set; } // 투표가 활성화되면 발생하는 이벤트 이름

    public VoteDetailData agree;
	public VoteDetailData disagree;
	public VoteDetailData abstention;

    int IDatabaseRow.ID { get { return id; } }

	int integer(string str)
	{
		if (str == "")
			return 0;

		return int.Parse(str);
	}

    bool IDatabaseRow.Parse(List<string> row)
    {
		Func<string, int> symbol = str => {
			str = str.Trim();
			str = str.Split(':')[0];
			if (str == "")
				return 0;

			return int.Parse(str.Substring(1));
		};

		Func<string, int> integer = str => {
			if (str == "")
				return 0;

			return int.Parse(str);
		};

		id = symbol(row[0]);
		day = integer(row[1]);
		voteTopic = row[3];

		var nextIfAgree = row[4].Split('/');
		agree.nextVote = symbol(nextIfAgree[0]);
		if (nextIfAgree.Length > 1)
			agree.nextVoteIfRobot = symbol(nextIfAgree[1]);

		var nextIfDisagree = row[5].Split('/');
		disagree.nextVote = symbol(nextIfDisagree[0]);
		if (nextIfDisagree.Length > 1)
			disagree.nextVoteIfRobot = symbol(nextIfDisagree[1]);

		var defaultSelection = row[6];

		agree.economy = integer(row[7]);
		agree.political = integer(row[8]);
		agree.mechanic = integer(row[9]);

		disagree.economy = integer(row[10]);
		disagree.political = integer(row[11]);
		disagree.mechanic = integer(row[12]);

		agree.action = symbol(row[13]);
		disagree.action = symbol(row[14]);
		eventName = row[15];

		abstention = disagree;
		if (defaultSelection == "예")
			abstention = agree;

		return true;
    }
}

public struct VoteDetailData
{
	public int nextVote { get; set; }
	public int nextVoteIfRobot { get; set; }
	public int economy { get; set; }
	public int political { get; set; }
	public int mechanic { get; set; }
	public int action; // 다음날이 되기 전에 일어나는 이벤트
}