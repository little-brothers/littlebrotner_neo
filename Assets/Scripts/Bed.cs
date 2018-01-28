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
		MyStatus.instance.Sleep();
	}

	void Start() {
		MyStatus.instance.AddSleepHook(vote => {
			VoteDetailData result = vote.disagree;
			if (vote.isAgree == 1)
				result = vote.agree;
			else if (vote.isAgree == -1)
				result = vote.abstention;

			switch (result.action) {
			case 1:
				break;

			case 2:
				MyStatus.instance.money.value += 3;
				break;

			case 3:
				MyStatus.instance.money.value -= 3;
				break;

			case 4:
				var item = 2;
				MyStatus.instance.inventory.Put(item, MyStatus.instance.inventory.GetItem(item) * 2);
				break;
			}
		});
	}
}
