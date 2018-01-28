using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TV04 : MonoBehaviour, IPointerClickHandler {
	struct News {
		public string article;
	}

	const string newsFile = "news";

	static List<News> news;

	// Use this for initialization
	void Start () {
		if (news == null) {
			// load once
			TV04.news = new List<News>();
			TextAsset csv = Resources.Load(newsFile) as TextAsset;
			if (csv == null) {
				Debug.Assert(false);
				return;
			}

			using (var streamReader = new StreamReader(new MemoryStream(csv.bytes)))
			{
				using (var reader = new Mono.Csv.CsvFileReader(streamReader))
				{
					// 0: id (N1)
					// 1: article
					List<string> row = new List<string>();
					reader.ReadRow(row); // first row is header

					// keep read content
					while(reader.ReadRow(row)) {
						var news = new News();
						news.article = row[1];

						TV04.news.Add(news);
					}
				}
			}

			Debug.Log("news loaded");
		}

		var currentNews = news[VoteManager.currentVote.id-1];
		transform.Find("Article").GetComponent<Text>().text = currentNews.article;
	}

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
		GameObject.Destroy(gameObject);
    }
}
