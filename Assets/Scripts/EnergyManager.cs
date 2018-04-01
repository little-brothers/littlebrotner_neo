using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyManager : MonoBehaviour {

	[SerializeField]
	float spacing = 0.02f;

	[SerializeField]
	SpriteRenderer notEnoughEnergy;

	[SerializeField]
	Sprite energyFilled; // 현재 사용 가능한 에너지 표시

	[SerializeField]
	Sprite energyWillFilled; // 다음날 충전될 에너지 표시

	public AudioClip sound;
	public AudioSource soundFx;

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
		MyStatus.instance.energyCharge.OnUpdate += _ => updateEnergyStatus(MyStatus.instance.energy);
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
			soundFx.PlayOneShot (sound);
			return false;
		}

		MyStatus.instance.energy.value--;
		return true;
	}

	public void updateEnergyStatus(int energy)
	{
		for (int i = 0; i < _energys.Count; ++i)
		{
			var child = _energys[i];
			if (i < energy) {
				child.sprite = energyFilled;
				child.color = Color.white;
			} else if (i < energy + MyStatus.instance.energyCharge) {
				child.sprite = energyWillFilled;
				child.color = new Color32(69, 255, 194, 120);
			} else {
				child.sprite = null;
			}
		}
	}
}
