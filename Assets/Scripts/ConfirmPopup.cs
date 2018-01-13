using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmPopup : MonoBehaviour {

	public delegate void PopupEvent();

	PopupEvent _okEvent;
	PopupEvent _cancelEvent;

	[SerializeField]
	Text _text;

	public static void Setup(string text, PopupEvent ok, PopupEvent cancel = null)
	{
		var popupTemplate = Resources.Load("ConfirmPopup");
		var popup = (GameObject.Instantiate(popupTemplate) as GameObject).GetComponent<ConfirmPopup>();
		popup._text.text = text;
		popup._okEvent = ok;
		popup._cancelEvent = cancel;

		popup.transform.SetParent(GameObject.FindGameObjectWithTag("RootCanvas").transform);
	}

	public void okButton()
	{
		if (_okEvent != null)
			_okEvent();

		GameObject.Destroy(gameObject);
	}

	public void cancelButton()
	{
		if (_cancelEvent != null)
			_cancelEvent();

		GameObject.Destroy(gameObject);
	}
}
