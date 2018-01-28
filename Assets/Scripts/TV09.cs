using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TV09 : MonoBehaviour {

	[SerializeField]
	TechListElement.Technology[] technologies;

	[SerializeField]
	VerticalLayoutGroup list;

	// Use this for initialization
	void Start () {
		var listElemTmpl = Resources.Load<GameObject>("TechListElement");
		foreach (var tech in technologies)
		{
			var elem = GameObject.Instantiate(listElemTmpl, list.transform).GetComponent<TechListElement>();
			elem.tech = tech;
		}
	}
}
