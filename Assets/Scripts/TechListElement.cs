using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TechListElement : ListElementBase {

	[Serializable]
	public struct Technology
	{
		public Sprite icon;
		public string name;
		public int progress;
	}

	public Technology tech
	{
		set {
			_icon.sprite = value.icon;
			_name.text = value.name;
			_progress.text = string.Format("{0}%", value.progress);
		}
	}

	Image _icon;
	Text _name;
	Text _progress;

	void Awake () {
		_icon = transform.Find("Icon").GetComponent<Image>();
		_name = transform.Find("Name").GetComponent<Text>();
		_progress = transform.Find("Progress").GetComponent<Text>();
	}
}
