using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[AssetPath("tech")]
public struct Technology : IDatabaseRow
{
	public int id;
	public Sprite icon;
	public string name;
	public string category;
	public string description;
	public int progress;
	public string condition;

	int IDatabaseRow.ID {get { return id; }}

	bool IDatabaseRow.Parse(List<string> row)
	{
		id = int.Parse(row[0].Substring(1));
		name = row[2];
		category = row[4];
		description = row[6];
		condition = row[8];

		return true;
	}
}

public class TechListElement : ListElementBase, IPointerClickHandler {

	public Technology tech
	{
		set {
			_tech = value;
			_name.text = value.name;
			_progress.text = value.category + " >";

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
	Technology _tech;

	void Awake () {
		_icon = transform.Find("Icon").GetComponent<Image>();
		_name = transform.Find("Name").GetComponent<Text>();
		_progress = transform.Find("Progress").GetComponent<Text>();
	}

	void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
	{
		ConfirmPopup.Setup(_tech.description, null, false);
	}
}
