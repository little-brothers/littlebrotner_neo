using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[AssetPath("work")]
public struct Work : IDatabaseRow {
	public int id;
	public string name;
	public string condition;
	public string description;
	public int[] payment;
	public int health;
	public string minigame;

    int IDatabaseRow.ID { get { return id; } }

    bool IDatabaseRow.Parse(List<string> row)
    {
		id = int.Parse(row[0].Substring(1));
		name = row[2];
		payment = row[3].Split(',').Select(str => int.Parse(str)).ToArray();
		health = int.Parse(row[4]);
		condition = row[5];
		description = row[7];
		minigame = row[10];

		return true;
    }
}

public class WorkListElement : ListElementBase, IPointerClickHandler {

	public delegate void OnJobSelectedEvent(Work work);

	public OnJobSelectedEvent OnJobSelected;

	Work _work;

	[SerializeField]
	List<Sprite> workIcons = new List<Sprite>();

	public Work work {
		set {
			_work = value;

			_name.text = _work.name;
			_health.text = string.Format("HP:{0}/G:{1}", _work.health, _work.payment[0]);
			// _health.text = _work.health.ToString();

			if (workIcons.Count >= _work.id)
				_icon.sprite = workIcons[_work.id-1];
		}
	}

	Text _health;
	Text _name;
	private Image _icon;

	// Use this for initialization
	void Awake () {
		_health = transform.Find("Health").GetComponent<Text>();
		_name = transform.Find("Name").GetComponent<Text>();
		_icon = transform.Find("Frame").Find("Icon").GetComponent<Image>();
	}

    public void OnPointerClick(PointerEventData eventData)
    {
		OnJobSelected(_work);
    }
}
