using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainControl : MonoBehaviour {

	private void Awake()
	{
		Screen.SetResolution(1920, 1080, true, 0);
		EncryptedPlayerPrefs.keys = new string[5];
        EncryptedPlayerPrefs.keys[0] = "fgfkjldjds";
        EncryptedPlayerPrefs.keys[1] = "tu849tu24";
        EncryptedPlayerPrefs.keys[2] = "40kf0rk";
        EncryptedPlayerPrefs.keys[3] = "jg05ji";
        EncryptedPlayerPrefs.keys[4] = "erjeioj";

		if (!EncryptedPlayerPrefs.HasKey("Ending1"))
		{
			for (int i = 1; i <= 20; i++)
			{
				EncryptedPlayerPrefs.SetInt("Ending" + i, 0);
				EncryptedPlayerPrefs.SaveEncryption("Ending" + i, "int", "littlebrother");
			}
		}

		// reset all ingame status
		MyStatus.Reset();
	}


	public void PressStory()
	{
		GameObject.Find("Soundmanager").GetComponent<Soundmanager> ().StoryPlay ();
		SceneManager.LoadScene("TrailerScene");
	}


	public void PressStart()
	{
		SceneManager.LoadScene("IntroScene");
		GameObject.Find("Soundmanager").GetComponent<Soundmanager> ().IntroPlay ();
		Debug.Log ("START");
	}

	public void PressAlbum()
	{
		SceneManager.LoadScene("AlbumScene");
	}

	public void PressCredit()
	{
		SceneManager.LoadScene("CreditScene_Main");
	}
		
	public void PressEnd()
	{
		Application.Quit();
	}
		
}
