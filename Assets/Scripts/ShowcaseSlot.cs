using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowcaseSlot : MonoBehaviour {

	Item _item;

	[SerializeField]
	Sprite[] itemShapes;

	public Item item {
		get { return _item; }
		set {
			_item = value;

			SpriteRenderer renderer = GetComponent<SpriteRenderer>();
			if (item == null) {
				renderer.sprite = null;
				return;
			}

			int spriteIdx = -1;
			switch (item.id) {
			case 2: spriteIdx = 0; break;
			case 3: spriteIdx = 1; break;
			case 4: spriteIdx = 2; break;
			case 7: spriteIdx = 3; break;
			}

			if (spriteIdx < 0) {
				renderer.sprite = null;
			} else {
				renderer.sprite = itemShapes[spriteIdx];
			}
		}
	}

	void Start() {

	}
}
