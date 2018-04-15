using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScriptBubble : MonoBehaviour, IPointerClickHandler {

	[SerializeField]
	public GameObject smallBubble;

	[SerializeField]
	public GameObject largeBubble;

	void Start()
	{
		// smallBubble.GetComponent<Animation>().Play();
		largeBubble.SetActive(false);
	}

	void toggleBubble()
	{
		if (smallBubble.activeSelf)
		{
			smallBubble.SetActive(false);
			largeBubble.SetActive(true);
		}
	}

	public void setText(string text)
	{
		largeBubble.transform.Find("Text").GetComponent<Text>().text = text;
	}

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
		toggleBubble();
    }
}
