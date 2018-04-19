using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameGauge : MonoBehaviour {

	const float MaxWidth = 115f;

	[SerializeField]
	GameObject mask;

	public float Progress
	{
		set {
			mask.transform.localScale = new Vector3(MaxWidth * value, 0.3f, 1);
		}
	}
}
