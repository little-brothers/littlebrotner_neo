using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템 저장고
public class Showcase : MonoBehaviour {

	List<ShowcaseSlot> _slots = new List<ShowcaseSlot>();
	
	// Use this for initialization
	void Start () {
		for (int i =0 ; i<transform.childCount; ++i) {
			var slot = transform.GetChild(i).GetComponent<ShowcaseSlot>();
			slot.OnSlotSelected += OnItemSelected;
			_slots.Add(slot);
		}

		// count should match the max inventory size
		Debug.Assert(_slots.Count == MyStatus.Inventory.MaxSize);

		// update inventory status when item changed
		MyStatus.instance.inventory.OnUpdate += refreshInventory;
		refreshInventory(null);
	}

	void refreshInventory(Item item) {
		var items = MyStatus.instance.inventory.slot;

		for (int i = 0; i < MyStatus.Inventory.MaxSize; ++i) {
			if (i < items.Count) {
				_slots[i].item = Database<Item>.instance.Find(items[i]);
			} else {
				_slots[i].item = null;
			}
		}
	}

	// 소모형 아이템 사용시 불리는 함수
	void OnItemSelected(Item item)
	{
		ConfirmPopup.Setup("Are you sure to use item " + item.name + "?", () => {
			if (!MyStatus.instance.inventory.Use(item)) {
				Debug.Log("failed to use item " + item.name);
				return;
			}

			switch (item.id) {
			case 2:
				MyStatus.instance.health.value += 5;
				if (MyStatus.instance.health > MyStatus.MaxHealth) {
					MyStatus.instance.health.value = MyStatus.MaxHealth;
				}

				MyStatus.instance.hunger.value = -1;
				break;

			case 3:
				MyStatus.instance.health.value += 10;
				if (MyStatus.instance.health > MyStatus.MaxHealth) {
					MyStatus.instance.health.value = MyStatus.MaxHealth;
				}

				MyStatus.instance.hunger.value = -1;
				break;

			case 4:
				MyStatus.instance.health.value += 2;
				if (MyStatus.instance.health > MyStatus.MaxHealth) {
					MyStatus.instance.health.value = MyStatus.MaxHealth;
				}

				if (Random.value < 0.25) {
					MyStatus.instance.sick.value = true;
				}

				MyStatus.instance.hunger.value = -1;
				break;

			case 7:
				MyStatus.instance.sick.value = false;
				break;
			}
		});
	}
}
