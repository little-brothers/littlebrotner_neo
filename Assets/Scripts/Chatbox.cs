using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Chatbox : MonoBehaviour, IPointerClickHandler {
	string _text;

	[SerializeField]
	Text textbox;

	public static void Show(string content)
	{
		var prefab = Resources.Load<GameObject>("ChatBox");
		var instance = GameObject.Instantiate(prefab).GetComponent<Chatbox>();
		instance.textbox.text = content;
		Utilities.SetUIParentFit(GameObject.FindWithTag("RootCanvas"), instance.gameObject);
	}

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
		GameObject.Destroy(gameObject);
    }
}
