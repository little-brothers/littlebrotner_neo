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
			StartCoroutine("Like");
		}
		else
		{
			StartCoroutine("Dislike");
		}
	}

	private IEnumerator Like()
	{
		float timer = 0f;
		while (timer <= 1f)
		{
			timer += Time.deltaTime * 1.5f;
			transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0f, 1f, 1f), timer);
			yield return null;
		}
		_image.sprite = _like;
		timer = 0f;
		while (timer <= 1f)
		{
			timer += Time.deltaTime * 1.5f;
			transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(-1f, 1f, 1f), timer);
			yield return null;
		}
		_likeCallback.Invoke();
	}

	private IEnumerator Dislike()
	{
		float timer = 0f;
		while (timer <= 1f)
		{
			timer += Time.deltaTime * 1.5f;
			transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0f, 1f, 1f), timer);
			yield return null;
		}
		_image.sprite = _dislike;
		timer = 0f;
		while (timer <= 1f)
		{
			timer += Time.deltaTime * 1.5f;
			transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1f, 1f, 1f), timer);
			yield return null;
		}
		_dislikeCallback.Invoke();
	}
}
