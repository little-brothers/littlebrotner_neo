using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WorkListElement : MonoBehaviour, IPointerClickHandler {

	public delegate void OnJobSelectedEvent(WorkListElement.Work work);

	public OnJobSelectedEvent OnJobSelected;

	[Serializable]
	public struct Work {
		public Sprite icon;
		public string name;
		public int payment;
		public int health;
		public bool enabled;
	}

	Work _work;
	public Work work {
		set {
			_work = value;
			_name.text = _work.name;
			_health.text = _work.health.ToString();
			_icon.sprite = _work.icon;
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
