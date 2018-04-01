using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstalledItem : MonoBehaviour {

	[SerializeField]
	int itemID;

	// Use this for initialization
	void Start() {
		gameObject.SetActive(false);
		MyStatus.instance.inventory.OnUpdate += item => {
			if (item.id == itemID)
				gameObject.SetActive(true);
		};
	}
}
