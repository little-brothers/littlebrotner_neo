using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class LikeButton : MonoBehaviour {

	[SerializeField]
	private Sprite _like;

	[SerializeField]
	private Sprite _dislike;

	[SerializeField]
	private UnityEvent _likeCallback;

	[SerializeField]
	private UnityEvent _dislikeCallback;

	private Image _image;

	private void Awake()
	{
		_image = GetComponent<Image>();
	}

	public void Touch()
	{
		StopAllCoroutines();
		if (_image.sprite.Equals(_dislike))
		{
			_image.sprite = _like;
			_likeCallback.Invoke();
		}
		else
		{
			_image.sprite = _dislike;
			_dislikeCallback.Invoke();
		}
	}
}
