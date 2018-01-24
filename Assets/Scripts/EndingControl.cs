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

	private void Start () 
	{
		LoadEndingData();
	}
	
	private void LoadEndingData()
	{
		_endingDatas = new List<EndingData>();
		TextAsset csv = Resources.Load("ending") as TextAsset;
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
				//Index 4: 제목
				//Index 9: 설명
				//Index 10: 힌트
				foreach(List<string> column in stringArray)
				{
					EndingData data = new EndingData();
					data.endingQuestion = column[2];
					data.endingHint = column[10];
					data.endingDescription = column[9];
					data.endingTitle = column[4];
					_endingDatas.Add(data);
				}
			}
		}
	}
}
