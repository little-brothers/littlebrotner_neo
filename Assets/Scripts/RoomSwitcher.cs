using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSwitcher : MonoBehaviour {

	[SerializeField]
	SpriteRenderer transitionPanel;

	protected int _index = 0;
	protected bool _inTransition = false;

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
		if (_inTransition)
			return;

		_index = idx % transform.childCount;
		if (_index < 0)
			_index += transform.childCount;

		StartCoroutine(fadeInOut(1.0f, _index));
	}

	void flipScene(int scene)
	{
		for (int i=0; i<transform.childCount; ++i)
		{
			var room = transform.GetChild(i);
			room.gameObject.SetActive(scene == i);
		}
	}

	IEnumerator fadeInOut(float duration, int scene)
	{
		_inTransition = true;
		if (transitionPanel != null)
		{
			// lock the screen
			transitionPanel.gameObject.SetActive(true);

			float endTime = Time.time + duration;
			float half = Time.time + duration/2;
			bool changed = false;
			var color = transitionPanel.color;
			while (Time.time < endTime)
			{
				float progress = Time.time - half;
				if (progress < 0)
				{
					progress = progress * -1;
				}
				else if (!changed)
				{
					changed = true;
					flipScene(scene);
				}

				color.a = 1 - (progress / (duration/2));
				transitionPanel.color = color;

				yield return null;
			}

			color.a = 0;
			transitionPanel.color = color;

			// unlock screen
			transitionPanel.gameObject.SetActive(false);
		}
		else
		{
			flipScene(scene);
		}

		_inTransition = false;
		yield break;
	}
}
