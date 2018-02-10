using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class energyWill : MonoBehaviour {

	[SerializeField]
	float spacing = 0.02f;

	public AudioClip sound;
	public AudioSource soundFx;

	List<SpriteRenderer> _energys_will = new List<SpriteRenderer>();
	public GameObject[] _e_w = new GameObject[12];

	// Use this for initialization
	void Start () {

		/*
		// place children
		for (int i = 0; i < transform.childCount; ++i)
		{
			var child = transform.GetChild(i).GetComponent<SpriteRenderer>();
			if (child == notEnoughEnergy)
				continue;

			_energys_will.Add(child);
	



			//array2 [i].SetActive (right);


		}
		*/
		MyStatus.instance.energy.OnUpdate += updateEnergyWillStatus;
		updateEnergyWillStatus(MyStatus.instance.energy);
	}

	void OnDestroy()
	{
		MyStatus.instance.energy.OnUpdate -= updateEnergyWillStatus;
	}


	void updateEnergyWillStatus(int ene)
	{
		int i = ene;
		int j = MyStatus.instance._energyCharge;
		int k = i + j;
		if (i+j > 12) {
			k = 12;
		}

		for (int a = 0; a < 12; a++) {
			Debug.Log (_e_w[a]);
			_e_w [a].SetActive (false);

		}


		for (int a = i; a < k; a++) {

			_e_w [a].SetActive (true);

		}

	}
}
