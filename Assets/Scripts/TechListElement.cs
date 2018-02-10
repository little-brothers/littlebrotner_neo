using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AssetPath("tech")]
public struct Technology : IDatabaseRow
{
	public int id;
	public Sprite icon;
	public string name;
	public int progress;
	public string condition;

    int IDatabaseRow.ID {get { return id; }}

    bool IDatabaseRow.Parse(List<string> row)
    {
		id = int.Parse(row[0].Substring(1));
		name = row[2];
		condition = row[9];

		return true;
    }
}

public class TechListElement : ListElementBase {

	public Technology tech
	{
		set {
			_name.text = value.name;
			_progress.text = string.Format("{0}%", value.progress);

			if (icons.Count >= value.id) {
				_icon.sprite = icons[value.id-1];
			}
		}
	}

	[SerializeField]
	List<Sprite> icons = new List<Sprite>();

	Image _icon;
	Text _name;
	Text _progress;

	void Awake () {
		_icon = transform.Find("Icon").GetComponent<Image>();
		_name = transform.Find("Name").GetComponent<Text>();
		_progress = transform.Find("Progress").GetComponent<Text>();
	}
}
