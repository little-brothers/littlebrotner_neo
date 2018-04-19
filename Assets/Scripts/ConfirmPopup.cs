using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmPopup : MonoBehaviour {

	public delegate void PopupEvent();

	PopupEvent _okEvent;

	[SerializeField]
	Text _text;
	[SerializeField]
	Button OkButton;
	[SerializeField]
	Button CancelButton;


	public static void Setup(string text, PopupEvent ok, bool showCancel = true)
	{
		var popupTemplate = Resources.Load("ConfirmPopup");
		var popup = (GameObject.Instantiate(popupTemplate) as GameObject).GetComponent<ConfirmPopup>();
		popup._text.text = text;
		popup._okEvent = ok;
		popup.CancelButton.gameObject.SetActive(showCancel);

		Utilities.SetUIParentFit(GameObject.FindGameObjectWithTag("RootCanvas"), popup.gameObject);
	}
	
	void Start() {
		OkButton.onClick.AddListener(() => okButton());
		CancelButton.onClick.AddListener(() => cancelButton());
	}

	public void okButton()
	{
		if (_okEvent != null)
			_okEvent();

		GameObject.Destroy(gameObject);
	}

	public void cancelButton()
	{
		GameObject.Destroy(gameObject);
	}
}
