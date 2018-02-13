using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Chatbox : MonoBehaviour, IPointerClickHandler {

	string _text;

	[SerializeField]
	Text textbox;

	public static Chatbox Show(string content)
	{
		var prefab = Resources.Load<GameObject>("ChatBox");
		var instance = GameObject.Instantiate(prefab).GetComponent<Chatbox>();
		instance.textbox.text = content;
		Utilities.SetUIParentFit(GameObject.FindWithTag("RootCanvas"), instance.gameObject);

		return instance;
	}

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
		GameObject.Destroy(gameObject);
    }
}
