using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSwitcher : MonoBehaviour {

	protected int _index = 0;
	protected bool _inTransition = false;

	// Use this for initialization
	void Start () {
		GetComponent<SpriteRenderer>().sortingOrder = 1; // 항상 최상위에 표시됨
		Debug.Assert(transform.childCount != 0);
		setRoomIdx(0);
	}
	
	public void nextRoom()
	{
		setRoomIdx(_index + 1);
	}

	public void prevRoom()
	{
		setRoomIdx(_index - 1);
	}

	protected void setRoomIdx(int idx)
	{
		if (_inTransition)
			return;

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
	}

	IEnumerator fadeInOut(float duration, int scene)
	{
		_inTransition = true;

		// lock the screen
		GetComponent<SpriteRenderer>().enabled = true;

		var anim = GetComponent<Animation>();
		anim.Play();

		while(anim.isPlaying)
			yield return null;

		// unlock screen
		GetComponent<SpriteRenderer>().enabled = false;

		_inTransition = false;
		yield break;
	}
}
