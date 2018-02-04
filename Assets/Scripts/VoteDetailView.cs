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
		switch (VoteManager.currentVote.choice) {
		case VoteSelection.Abstention:
		case VoteSelection.NotYet:
			acceptButton.transform.Find("Selected").gameObject.SetActive(false);
			declineButton.transform.Find("Selected").gameObject.SetActive(false);
			break;

		case VoteSelection.Decline:
			acceptButton.transform.Find("Selected").gameObject.SetActive(false);
			break;

		case VoteSelection.Accept:
			declineButton.transform.Find("Selected").gameObject.SetActive(false);
			break;
		}
	}

	public void OnAnswer(bool accept)
	{
		Debug.Log(accept.ToString());
		VoteManager.Vote(accept ? VoteSelection.Accept : VoteSelection.Decline);
		GameObject.Destroy(gameObject);
	}
}
