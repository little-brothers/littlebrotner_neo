using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animation))]
public class IntroSceneControl : MonoBehaviour {

	[Serializable]
	struct Step
	{
		public Sprite image;
		public string script;
	}

	private Animation anim;
	private AnimationClip[] clips;
	int _current = -1;

	[SerializeField]
	List<Step> sequence = new List<Step>();

	[SerializeField]
	public GameObject linkClick;

	IEnumerator _waitScriptHandle = null;
	Chatbox _scriptBox = null;


	//private Button qBtn;

	// Use this for initialization








	void Start () {
		GameMusic();
		TriggerFadeout();
	}

	public void nextStepNow()
	{
		if (_current >= sequence.Count)
		{
			GameObject.Find("Soundmanager").GetComponent<Soundmanager> ().MainPlay ();
			SceneManager.LoadScene("GameScene");
			return;
		}

		Debug.Log(_current);
		GetComponent<SpriteRenderer>().sprite = sequence[_current].image;
	}









	public void ShowScript()
	{
		if (_scriptBox != null)
			GameObject.Destroy(_scriptBox.gameObject);

		if (_waitScriptHandle != null)
			StopCoroutine(_waitScriptHandle);

		_waitScriptHandle = ShowScriptInternal();
		StartCoroutine(_waitScriptHandle);
	}

	IEnumerator ShowScriptInternal()
	{
		_scriptBox = Chatbox.Show(sequence[_current].script);
		while (_scriptBox != null) yield return null;

		TriggerFadeout();
	}













	public void PressSkip()
	{
		// 페이드 애니메이션 중에는 재생하지 않음
		if (GetComponent<Animation>().isPlaying)
			return;

		_current = sequence.Count-1;
		TriggerFadeout();
	}

	public void GameMusic(){

		// GameObject.Find("Soundmanager").GetComponent<Soundmanager> ().MainPlay ();

	}

	void TriggerFadeout()
	{
		_current++;
		GetComponent<Animation>().Play(PlayMode.StopAll);
	}
}


