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
	public string cost;

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
		cost = row [11];

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
			_available = MyStatus.Check(_work.condition);

			_name.text = _work.name;
			_health.text = string.Format("HP:{0} / G:{1}", _work.health, _work.cost);
			// _health.text = _work.health.ToString();

			if (workIcons.Count >= _work.id)
				_icon.sprite = workIcons[_work.id-1];

			if (!_available) {
				_health.color = halfAlpha(_health.color);
				_name.color = halfAlpha(_name.color);
				_icon.color = halfAlpha(_icon.color);
				_frame.color = halfAlpha(_frame.color);
			}
		}
	}

	Text _health;
	Text _name;
	Image _icon;
	Image _frame;
	bool _available;

	// Use this for initialization
	void Awake () {
		_health = transform.Find("Health").GetComponent<Text>();
		_name = transform.Find("Name").GetComponent<Text>();
		_frame = transform.Find("Frame").GetComponent<Image>();
		_icon = _frame.transform.Find("Icon").GetComponent<Image>();
	}

	Color halfAlpha(Color original)
	{
		original.a = 0.5f;
		return original;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (_available)
			OnJobSelected(_work);
	}
}
