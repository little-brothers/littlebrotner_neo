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
		var instance = GameObject.Instantiate(
			prefab,
			Vector3.zero,
			Quaternion.identity,
			GameObject.FindWithTag("RootCanvas").transform.parent
		).GetComponent<Chatbox>();

		instance.textbox.text = content;

		return instance;
	}

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
		GameObject.Destroy(gameObject);
    }
}
