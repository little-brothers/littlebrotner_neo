
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditSceneController : MonoBehaviour {

	void Start(){

		GameObject.Find("Soundmanager").GetComponent<Soundmanager> ().CreditPlay ();
	}

    public void ToMainMenu()
    {
        MyStatus.Reset();
        SceneManager.LoadScene("MainScene");
    }
}