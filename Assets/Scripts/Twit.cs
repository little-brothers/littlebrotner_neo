using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Twit : MonoBehaviour {

	[SerializeField]
	private List<Sprite> _profiles;

	[SerializeField]
	private Image _profile;

	[SerializeField]
	private Text _twit;

	[SerializeField]
	private LikeButton _likeButton;

	public void SetProfile(int index)
	{
		Debug.Assert(_profiles.Count <= index);
		_profile.sprite = _profiles[index];
	}

	public void SetTwit(string twit)
	{
		_twit.text = twit;
	}
}
