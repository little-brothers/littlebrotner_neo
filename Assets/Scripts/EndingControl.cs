using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public struct EndingData
{
	public string endingQuestion { set; get; }
	public string endingTitle { set; get; }
	public string endingDescription { set; get; }
	public string endingHint {set; get; }
}

public class EndingControl : MonoBehaviour {

	private List<EndingData> _endingDatas;

	void Start () {
		_endingDatas = new List<EndingData>();
		TextAsset csv = Resources.Load("ending") as TextAsset;
		using (var streamReader = new StreamReader(new MemoryStream(csv.bytes)))
		{
			using (var reader = new Mono.Csv.CsvFileReader(streamReader))
			{
				var stringArray = new List<List<string>>();
				reader.ReadAll(stringArray);
				stringArray.RemoveAt(0);

				//Index 2: 현재 투표 인덱스
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
				foreach(List<string> column in stringArray)
				{

					//_voteDatas.Add(GenerateVoteData(column));
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
