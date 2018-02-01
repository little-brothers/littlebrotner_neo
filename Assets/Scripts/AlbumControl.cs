using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AlbumControl : MonoBehaviour {

	[SerializeField]
	private SpriteRenderer[] _screens;

	[SerializeField]
	private Sprite[] _endingThumbnails;

	private void Start () 
	{
		for (int i = 1; i <= 20; ++i)
		{
			string key = "Ending" + i;
			if (EncryptedPlayerPrefs.GetInt(key).Equals(1))
				_screens[i - 1].sprite = _endingThumbnails[i - 1];
		}
	}

	public void PressMain()
	{
		SceneManager.LoadScene("MainScene");
	}
}
