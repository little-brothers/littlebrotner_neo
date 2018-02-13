using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TV09 : MonoBehaviour {
	[SerializeField]
	VerticalLayoutGroup list;

	// Use this for initialization
	void Start () {
		var available = Database<Technology>.instance.ToList()
							.Where(tech => MyStatus.instance.technologies.Contains(tech.id))
							.Reverse();

		var listElemTmpl = Resources.Load<GameObject>("TechListElement");
		foreach (var tech in available)
		{
			var elem = GameObject.Instantiate(listElemTmpl, list.transform).GetComponent<TechListElement>();
			elem.tech = tech;
		}
	}
}
