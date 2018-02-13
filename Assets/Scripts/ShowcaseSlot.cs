using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowcaseSlot : MonoBehaviour, IPointerClickHandler {
	public delegate void SlotSelectedEvent(Item item);
	public event SlotSelectedEvent OnSlotSelected;

	Item _item;

	[SerializeField]
	Sprite[] itemShapes;

	public Item item {
		get { return _item; }
		set {
			_item = value;

			SpriteRenderer renderer = GetComponent<SpriteRenderer>();
			BoxCollider2D collider = GetComponent<BoxCollider2D>();
			if (item == null) {
				renderer.sprite = null;
				collider.enabled = false;
				return;
			}

			collider.enabled = true;

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

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
		if (item == null)
			return;

		OnSlotSelected(item);
    }

	public void ShowTooltip(Tooltip tooltip)
	{
		tooltip.Hide(); // hack
		if (item != null)
			tooltip.Show(item.name);
	}

	public void HideTooltip(Tooltip tooltip)
	{
		tooltip.Hide();
		tooltip.Show("Cabinet"); // hack
	}

}
