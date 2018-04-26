using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class trailerControl : MonoBehaviour {

	public void PressMain()
	{
		SceneManager.LoadScene("MainScene");
	}
}
