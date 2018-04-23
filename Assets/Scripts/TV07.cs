using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

[AssetPath("history")]
struct History : IDatabaseRow
{
	public int id;
	public string sprites;
	public string script;

    int IDatabaseRow.ID
	{
		get { return id; }
	}

    bool IDatabaseRow.Parse(List<string> row)
    {
		id = int.Parse(row[0].Substring(1));
		sprites = row[2];
		script = row[3];

		return true;
    }
}

public class TV07 : MonoBehaviour {

	[SerializeField]
	private Transform _gridParent;

	[SerializeField]
	private Text _script;

	private void Start()
	{
		SetData();
	}

	private void SetData()
	{
		History history = Database<History>.instance.Find(VoteManager.currentVote.id);

		// races
		if (history.sprites.Trim().Length != 0)
		{
			string[] sprites = history.sprites.Split('/');
			for (int i = 0; i < sprites.Length; ++i)
			{
				GameObject temp = new GameObject();
				temp.transform.parent = _gridParent;
				temp.transform.localScale = Vector3.one;
				Image image = temp.AddComponent<Image>();
				image.sprite = Resources.Load<Sprite>("TV07/" + sprites[i]);
			}
		}

		// script
		_script.text = history.script;
	}
}
