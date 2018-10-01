using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomSwitcher : MonoBehaviour {

	public delegate void OnSceneChangeEvent();

	protected event OnSceneChangeEvent _reservedChangeEvent;
	protected int _index = 0;
	protected bool _inTransition = false;

	[SerializeField]
	int startIndex = 0;

	[SerializeField]
	bool loop = true;

	[SerializeField]
	Tooltip tooltip;




	// Use this for initialization
	void Start () {

		GetComponent<SpriteRenderer>().sortingOrder = 1; // 항상 최상위에 표시됨
		Debug.Assert(transform.childCount != 0);
		setRoomIdx(startIndex);


		// turn off all rooms
		for (int i=0; i<transform.childCount; ++i)
		{
			var room = transform.GetChild(i);
			// room.gameObject.SetActive(false);
		}
	}

	void ShowPendingNotis()
	{
		var notis = MyStatus.instance.GetAndClearNotifications();

		// temp
		// StartCoroutine(ShowPendingNotisInternal(notis));
	}

	IEnumerator ShowPendingNotisInternal(List<Notification> notis)
	{
		if (notis == null)
			yield break;

		foreach (var noti in notis)
		{
			var box = Chatbox.Show(noti.text);
			while (box) yield return null;
		}
	}
	
	public void setRoomIdx(int idx, OnSceneChangeEvent OnSceneWillChange = null, OnSceneChangeEvent OnSceneChanged = null)
	{
		if (_inTransition)
			return;

		// 루프 방지
		if (!loop && (idx < 0 || idx >= transform.childCount))
			return;

		_reservedChangeEvent = OnSceneWillChange;
		_index = idx % transform.childCount;
		if (_index < 0)
			_index += transform.childCount;

		StartCoroutine(fadeInOut(1.0f, _index, OnSceneChanged));
	}

	public void setRoomIdxImmediate(int idx)
	{
		_index = idx % transform.childCount;
		if (_index < 0)
			_index += transform.childCount;

		flipScene(false);
	}

	void flipSceneNow() {
		flipScene(true);
	}

	void flipScene(bool fromAnimation)
	{
		for (int i=0; i<transform.childCount; ++i)
		{
			var room = transform.GetChild(i);
			// room.gameObject.SetActive(_index == i);
		}

		if (fromAnimation && _reservedChangeEvent != null) {
			_reservedChangeEvent();
		}

		if (tooltip != null)
			tooltip.Hide();
	}

	IEnumerator fadeInOut(float duration, int scene, OnSceneChangeEvent finishEvent)
	{
		_inTransition = true;

		var anim = GetComponent<Animation>();
		anim.Play();

		while(anim.isPlaying)
			yield return null;

		_inTransition = false;
		if (finishEvent != null)
			finishEvent();

		yield break;
	}
}
