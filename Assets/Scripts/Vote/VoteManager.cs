using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class VoteManager {

	private static List<VoteData> _voteDatas;

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
				//foreach (var words in stringArray)
                for (int i = 0; i < stringArray.Count; ++i)
				{
                    if (stringArray[i].Count.Equals(0))
                        continue;

					VoteData voteData = new VoteData();
					voteData.nextVoteIndex = i; // 나중에 추가되면 변경하는 걸로 바꾸기
					voteData.voteDivision = stringArray[i][0];
					voteData.voteTopic = stringArray[i][2];
					voteData.voteResult = stringArray[i][3];
					//Debug.Log("Division: " + voteData.voteDivision + " | Topic: " + voteData.voteTopic + " | Result:" + voteData.voteResult);
					_voteDatas.Add(voteData);
                }
			}
		}
		return true;
	}
}
