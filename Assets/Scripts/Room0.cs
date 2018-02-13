using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Room0 : MonoBehaviour {

	[SerializeField]
	Color normalColor;

	[SerializeField]
	Color warnColor;

	[SerializeField]
	Color alertColor;

	SpriteRenderer _background;
	SpriteRenderer _backgroundLight;

	// Use this for initialization
	void Awake () {
		_background = transform.Find("Background").GetComponent<SpriteRenderer>();
		_backgroundLight = _background.transform.Find("Light").GetComponent<SpriteRenderer>();

		// 
		MyStatus.instance.homeDestroyed.OnUpdate += value => UpdateAlertStatus();
		MyStatus.instance.plague.OnUpdate += value => UpdateAlertStatus();
		MyStatus.instance.invasion.OnUpdate += value => UpdateAlertStatus();

		// 자가 수리 키트 확인
		MyStatus.instance.inventory.OnUpdate += checkRepairItem;

		UpdateAlertStatus();
	}
	
	public void ConfirmExit()
	{
		ConfirmPopup.Setup("Are you sure to exit the room? (game over)", () => {
			GameObject.FindObjectOfType<RoomSwitcher>().setRoomIdx(0, () => {
				MyStatus.instance.ResetAllHooks();
				SceneManager.LoadScene("MainScene");
			});
		});
	}

	void UpdateAlertStatus()
	{
		int level = MyStatus.instance.invasion;
		if (MyStatus.instance.homeDestroyed)
			level = 2;

		if (MyStatus.instance.plague)
			level = 2;

		Color color = normalColor;
		if (level == 1)
			color = warnColor;
		else if (level == 2)
			color = alertColor;

		_background.color = color;
		_backgroundLight.color = color;
	}

	void checkRepairItem(Item item)
	{
		if (item.id == 11)
		{
			bool repaired = MyStatus.instance.homeDestroyed;
			MyStatus.instance.homeDestroyed.value = false;

			if (repaired)
				Chatbox.Show("My house has repaired");
		}
	}
}
