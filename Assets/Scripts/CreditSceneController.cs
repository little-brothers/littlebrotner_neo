
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditSceneController : MonoBehaviour {

    public void ToMainMenu()
    {
        MyStatus.Reset();
        SceneManager.LoadScene("MainScene");
    }
}