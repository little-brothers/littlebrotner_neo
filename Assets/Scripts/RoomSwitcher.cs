using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSwitcher : MonoBehaviour {

	protected int _index = 0;

	// Use this for initialization
	void Start () {
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
		_index = idx % transform.childCount;
		if (_index < 0)
			_index += transform.childCount;

		for (int i=0; i<transform.childCount; ++i)
		{
			var room = transform.GetChild(i);
			room.gameObject.SetActive(_index == i);
		}
	}
}
