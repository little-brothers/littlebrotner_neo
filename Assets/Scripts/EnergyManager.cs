using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyManager : MonoBehaviour {

	[SerializeField]
	float spacing = 0.02f;

	[SerializeField]
	SpriteRenderer notEnoughEnergy;

	List<SpriteRenderer> _energys = new List<SpriteRenderer>();

	// Use this for initialization
	void Start () {
		notEnoughEnergy.enabled = false;

		// place children
		for (int i = 0; i < transform.childCount; ++i)
		{
			var child = transform.GetChild(i).GetComponent<SpriteRenderer>();
			if (child == notEnoughEnergy)
				continue;

			_energys.Add(child);
			child.transform.localPosition = Vector3.down * (spacing + child.size.y) * (_energys.Count-1);
		}

		MyStatus.instance.energy.OnUpdate += updateEnergyStatus;
		updateEnergyStatus(MyStatus.instance.energy);
	}

	void OnDestroy()
	{
		MyStatus.instance.energy.OnUpdate -= updateEnergyStatus;
	}

	public bool UseEnergy()
	{
		if (MyStatus.instance.energy == 0) {
			GetComponent<Animation>().Play(PlayMode.StopAll);
			return false;
		}

		MyStatus.instance.energy.value--;
		return true;
	}

	void updateEnergyStatus(int energy)
	{
		for (int i = 0; i < _energys.Count; ++i)
		{
			var child = _energys[i];
			child.gameObject.SetActive(i < energy);
		}
	}
}
