
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CreditSceneController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	void Start(){
        var sound = GameObject.Find("Soundmanager");
        if (sound != null)
            sound.GetComponent<Soundmanager>().CreditPlay ();
	}

    public void ToMainMenu()
    {
        SceneManager.LoadScene("LogoScene");
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        Time.timeScale = 1f;
        // GetComponent<Animation>()["crdit"].speed = 1f;
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        Time.timeScale = 10f;
        // GetComponent<Animation>()["credit"].speed = 10f;
    }
}