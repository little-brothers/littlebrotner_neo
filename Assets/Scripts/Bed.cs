using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bed : MonoBehaviour, IPointerDownHandler {
    public void OnPointerDown(PointerEventData eventData)
    {
		ConfirmPopup.Setup("So sleepy... go to bed?", gotoSleep);
    }

	void gotoSleep()
	{
		var tvs = Utilities.FindObjectsOfType<Television>();
		foreach (var tv in tvs)
			tv.ResetWatched();

		Utilities.FindObjectOfType<EnergyManager>().ResetEnergy();
		VoteManager.NextDay();
	}
}
