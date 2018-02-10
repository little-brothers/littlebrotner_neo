using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomSwitcher : MonoBehaviour {

	public delegate void OnSceneNeedsChange();

	protected event OnSceneNeedsChange _reservedChangeEvent;
	protected int _index = 0;
	protected bool _inTransition = false;

	[SerializeField]
	int startIndex = 0;

	[SerializeField]
	bool loop = true;

	[SerializeField]
	Button leftButton;

	[SerializeField]
	Button rightButton;

	[SerializeField]
	Tooltip tooltip;




	// Use this for initialization
	void Start () {

		//updateButtonInteractable();
		//updateButtonInteractable();
		GetComponent<SpriteRenderer>().sortingOrder = 1; // 항상 최상위에 표시됨
		Debug.Assert(transform.childCount != 0);
		setRoomIdx(startIndex);


		// turn off all rooms
		for (int i=0; i<transform.childCount; ++i)
		{
			var room = transform.GetChild(i);
			room.gameObject.SetActive(false);
		}


		leftButton.onClick.AddListener(() => {
			setRoomIdx(_index-1);
			updateButtonInteractable();
		});

		rightButton.onClick.AddListener(() => {
			setRoomIdx(_index+1);
			updateButtonInteractable();
		});
	}

	void updateButtonInteractable()
	{
		leftButton.interactable = _index > 0;
		rightButton.interactable = _index < transform.childCount-1;
	}
	
	public void setRoomIdx(int idx, OnSceneNeedsChange OnSceneChange = null)
	{
		if (_inTransition)
			return;

		// 루프 방지
		if (!loop && (idx < 0 || idx >= transform.childCount))
			return;

		_reservedChangeEvent = OnSceneChange;
		_index = idx % transform.childCount;
		if (_index < 0)
			_index += transform.childCount;

		StartCoroutine(fadeInOut(1.0f, _index));
	}

	void flipSceneNow()
	{
		for (int i=0; i<transform.childCount; ++i)
		{
			var room = transform.GetChild(i);
			room.gameObject.SetActive(_index == i);
		}

		if (_reservedChangeEvent != null) {
			_reservedChangeEvent();
		}

		if (tooltip != null)
			tooltip.Hide();
	}

	IEnumerator fadeInOut(float duration, int scene)
	{
		_inTransition = true;

		var anim = GetComponent<Animation>();
		anim.Play();

		while(anim.isPlaying)
			yield return null;

		_inTransition = false;
		yield break;
	}
}
