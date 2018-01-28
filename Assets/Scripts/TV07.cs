using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class TV07 : MonoBehaviour {

	struct History
	{
		public string sprites;
		public string script;
	}

	[SerializeField]
	private Transform _gridParent;

	[SerializeField]
	private Text _script;

	private static List<History> _historyDatas = null;

	private void Awake()
	{
		LoadHistoryData();
	}

	private void Start()
	{
		SetData();
	}

	private void LoadHistoryData()
	{
		if (_historyDatas != null)
			return;

		_historyDatas = new List<History>();
		TextAsset csv = Resources.Load("history") as TextAsset;
		using (var streamReader = new StreamReader(new MemoryStream(csv.bytes)))
		{
			using (var reader = new Mono.Csv.CsvFileReader(streamReader))
			{
				var stringArray = new List<List<string>>();
				reader.ReadAll(stringArray);
				stringArray.RemoveAt(0);

				//Index 2: 이미지
				//Index 3: 스크립트
				foreach(List<string> column in stringArray)
				{
					History data = new History();
					data.sprites = column[2];
					data.script = column[3];
					_historyDatas.Add(data);
				}
			}
		}
	}

	private void SetData()
	{
		History history = _historyDatas[VoteManager.currentVote.id - 1];
		string[] sprites = history.sprites.Split('/');
		for (int i = 0; i < sprites.Length; ++i)
		{
			GameObject temp = new GameObject();
			temp.transform.parent = _gridParent;
			temp.transform.localScale = Vector3.one;
			Image image = temp.AddComponent<Image>();
			image.sprite = Resources.Load<Sprite>("TV07/" + sprites[i]);
		}
		_script.text = history.script;
	}
}
