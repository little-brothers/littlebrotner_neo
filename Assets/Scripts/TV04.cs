using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class TV04 : MonoBehaviour, IPointerClickHandler {

	[AssetPath("news")]
	struct News : IDatabaseRow
	{
		public int id;
		public string article;

        int IDatabaseRow.ID { get { return id; } }

        bool IDatabaseRow.Parse(List<string> row)
        {
			id = int.Parse(row[0].Substring(1));
			article = row[2];

			return true;
        }
    }

	// Use this for initialization
	void Start () {
		var currentNews = Database<News>.instance.Find(VoteManager.currentVote.id);
		transform.Find("Article").GetComponent<Text>().text = currentNews.article;
	}

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
		GameObject.Destroy(gameObject);
    }
}
