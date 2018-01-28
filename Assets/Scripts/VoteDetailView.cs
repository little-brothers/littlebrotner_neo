using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoteDetailView : MonoBehaviour {

	[SerializeField]
	Text questionText;

	// Use this for initialization
	void Start () {
		questionText.text = VoteManager.currentVote.voteTopic;
	}

	public void OnAnswer(bool accept)
	{
		Debug.Log(accept.ToString());
		VoteManager.Vote(accept ? 1 : 0);
		GameObject.Destroy(gameObject);
	}
}
