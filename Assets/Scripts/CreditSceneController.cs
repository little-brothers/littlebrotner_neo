
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditSceneController : MonoBehaviour {

    public void ToMainMenu()
    {
        SceneManager.LoadScene("MainScene");
    }
}