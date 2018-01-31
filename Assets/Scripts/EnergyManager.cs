using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyManager : MonoBehaviour {

	[SerializeField]
	float spacing = 0.02f;

	// Use this for initialization
	void Start () {
		// place children
		for (int i = 0; i < transform.childCount; ++i)
		{
			var child = transform.GetChild(i);
			child.localPosition = Vector3.down * (spacing + child.GetComponent<SpriteRenderer>().size.y) * i;
			//Debug.Log(child.localPosition.ToString());
		}

		MyStatus.instance.energy.OnUpdate += updateEnergyStatus;
		updateEnergyStatus(MyStatus.instance.energy);
	}

	public bool UseEnergy()
	{
		if (MyStatus.instance.energy == 0)
			return false;

		MyStatus.instance.energy.value--;
		return true;
	}

	void updateEnergyStatus(int energy)
	{
		for (int i = 0; i < transform.childCount; ++i)
		{
			var child = transform.GetChild(i);
			child.gameObject.SetActive(i < energy);
		}
	}
}
