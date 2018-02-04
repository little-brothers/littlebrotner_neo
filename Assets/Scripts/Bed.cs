using System;
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
		var switcher = GameObject.FindObjectOfType<RoomSwitcher>();
		switcher.setRoomIdx(2, () => MyStatus.instance.Sleep());
	}

	void applyEvent9(VoteData vote, MyStatus.Snapshot status) {
		MyStatus.instance.money.value -= 1;
		MyStatus.instance.energy.value += 1;
	}

	void Start() {
		MyStatus.instance.AddSleepHook((vote, status) => {
			VoteDetailData result = vote.disagree;
			if (vote.choice == VoteSelection.Accept)
				result = vote.agree;
			else if (vote.choice == VoteSelection.Abstention)
				result = vote.abstention;

			// switch의 각 숫자 설정
			// E1:W4의 돈 두배	
			// E2:돈 +3
			// E3:돈 -3
			// E4:I2 + 2개
			// E5:쓰지않은 전기는 모두 금화가 되는 규칙	
			// E6:아이템 : 세금 1개 늘어나고 식량 1개받음	
			// E7:아이템 : 즉시)돈7개 이상인 사람 3개씩 거둠	
			// E8:8freedom에서 로봇 아이콘이 등장	
			// E8:규칙 : 세금 2개 추가!	
			// E9:규칙 : 매일 돈 1개 거두고 / 전기 1개 제공	
			// E10: 다음날 금화 3개
			// E11: 3일뒤 9개
			switch (result.action) {
			case 1:
				throw new NotImplementedException("double payment for work 4");

			case 2:
				MyStatus.instance.money.value += 3;
				break;

			case 3:
				MyStatus.instance.money.value -= 3;
				break;

			case 4:
				var item = Database<Item>.instance.Find(2);
				MyStatus.instance.inventory.Put(item);
				MyStatus.instance.inventory.Put(item);
				break;

			case 5:
				MyStatus.instance.money.value += status.energy;
				MyStatus.instance.energy.value -= status.energy;
				break;

			case 6:
				throw new NotImplementedException("add a tax and add a food supply");

			case 7:
				if (MyStatus.instance.money >= 7)
					MyStatus.instance.money.value -= 3;

				break;

			case 8:
				throw new NotImplementedException("add 2 taxes!");

			case 9:
				// 앞으로 매일 밤마다 실행
				MyStatus.instance.AddSleepHook(applyEvent9);
				break;

			case 10:
				MyStatus.instance.money.value += 3;
				break;

			case 11:
				int day = 0;
				MyStatus.instance.AddSleepHook((_vote, _status) => {
					day++;
					if (day == 3) {
						MyStatus.instance.money.value += 9;
					}
				});
				break;
			}
		});
	}
}
