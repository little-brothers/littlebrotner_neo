using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyManager : MonoBehaviour {

	[SerializeField]
	float spacing = 0.02f;

	const int energyLimit = 12;
	int _maxEnergy = 4;
	int _currentEnergy;

	// Use this for initialization
	void Start () {
		_currentEnergy = _maxEnergy;

		// place children
		for (int i = 0; i < transform.childCount; ++i)
		{
			var child = transform.GetChild(i);
			child.localPosition = Vector3.down * (spacing + child.GetComponent<SpriteRenderer>().size.y) * i;
			//Debug.Log(child.localPosition.ToString());
		}

		// 다음날에 에너지 정리
		MyStatus.instance.AddSleepHook(vote => ResetEnergy());
		updateEnergyStatus();
	}

	public void ResetEnergy()
	{
		_currentEnergy = Mathf.Min(_currentEnergy+_maxEnergy, energyLimit);
		updateEnergyStatus();
	}
	
	public bool UseEnergy()
	{
		if (_currentEnergy == 0)
			return false;

		_currentEnergy--;
		updateEnergyStatus();
		return true;
	}

	void updateEnergyStatus()
	{
		for (int i = 0; i < transform.childCount; ++i)
		{
			var child = transform.GetChild(i);
			child.gameObject.SetActive(i < _currentEnergy);
		}
	}
}
