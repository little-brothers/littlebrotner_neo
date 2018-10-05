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

	private List<BoxCollider2D> _blockers = null;

	// Use this for initialization
	void Start () {
		if (_blockers == null)
		{
			_blockers = new List<BoxCollider2D>();
			GameObject[] objects = GameObject.FindGameObjectsWithTag("TouchBlocker");
			foreach(GameObject obj in objects)
			{
				BoxCollider2D box = obj.GetComponent<BoxCollider2D>();
				box.enabled = true;
				_blockers.Add(box);
			}
		}

		questionText.text = Database<VoteData>.instance.Find(VoteManager.currentVote.id).voteTopic;
		switch (VoteManager.currentVote.selection) {
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

	void OnDestroy()
	{
		foreach(BoxCollider2D obj in _blockers)
		{
			obj.enabled = false;
		}
	}

	public void OnAnswer(bool accept)
	{
		Debug.Log(accept.ToString());
		VoteManager.Vote(accept ? VoteSelection.Accept : VoteSelection.Decline);
		GameObject.Destroy(gameObject);
	}
}
