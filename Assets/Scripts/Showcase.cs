using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템 저장고
public class Showcase : MonoBehaviour {

	List<ShowcaseSlot> _slots = new List<ShowcaseSlot>();
	
	// Use this for initialization
	void Start () {
		for (int i =0 ; i<transform.childCount; ++i) {
			_slots.Add(transform.GetChild(i).GetComponent<ShowcaseSlot>());
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
}
