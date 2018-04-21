using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameGauge : MonoBehaviour {

	const float MaxWidth = 115f;
	const float WidthUnit = 5f; // MaxWidth의 약수여야 모양이 예쁘게 나옴

	[SerializeField]
	GameObject mask;

	[SerializeField]
	bool discrete = false;

	public float Progress
	{
		set {
			float width = MaxWidth * value;
			if (discrete)
				width = Mathf.Min(MaxWidth, Mathf.Ceil(width / WidthUnit) * WidthUnit);

			mask.transform.localScale = new Vector3(width, 0.3f, 1);
		}
	}
}
