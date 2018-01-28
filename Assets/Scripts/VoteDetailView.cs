using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoteDetailView : MonoBehaviour {

	[SerializeField]
	Text questionText;

	[SerializeField]
	Button acceptButton;

	[SerializeField]
	Button declineButton;

	// Use this for initialization
	void Start () {
		questionText.text = VoteManager.currentVote.voteTopic;
		switch (VoteManager.currentVote.isAgree) {
		case -1:
			acceptButton.transform.Find("Selected").gameObject.SetActive(false);
			declineButton.transform.Find("Selected").gameObject.SetActive(false);
			break;

		case 0: // decline
			acceptButton.transform.Find("Selected").gameObject.SetActive(false);
			break;

		case 1:
			declineButton.transform.Find("Selected").gameObject.SetActive(false);
			break;
		}
	}

	public void OnAnswer(bool accept)
	{
		Debug.Log(accept.ToString());
		VoteManager.Vote(accept ? 1 : 0);
		GameObject.Destroy(gameObject);
	}
}
