using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameController : MonoBehaviour {

	[Serializable]
	public struct MinigameDesc {
		public string name;
		public GameObject game;
	}

	[SerializeField]
	List<MinigameDesc> minigames;

	[SerializeField]
	string game; // 게임씬에서 넘어오지 않은 경우에만(디버그) 사용

	[SerializeField]
	MinigameGauge timeGauge;

	[SerializeField]
	MinigameGauge scoreGauge;

	IMinigame _currentGame = null; // 현재 진행중인 미니게임
	float _endTime;

	// Use this for initialization
	void Start () {
		string gameName = game;
		Work work = Database<Work>.instance.Find(MyStatus.instance.lastWorkId);
		if (work.id != 0)
			gameName = work.minigame;

		MinigameDesc gameToPlay = minigames.Find(desc => desc.name == gameName);
		if (gameToPlay.game != null) {
			Debug.Log("found game " + gameToPlay.name);
			_currentGame = gameToPlay.game.GetComponent<IMinigame>();
			if (_currentGame == null) {
				throw new System.Exception(string.Format("minigame {0} is not implementing IMinigame", gameName));
			}

			// enable/disable games
			foreach (var desc in minigames) {
				desc.game.SetActive(desc.game == gameToPlay.game);
			}

			_currentGame.Setup();
			_endTime = Time.time + _currentGame.MaxTime;

			UpdateGauges();
		} else {
			Debug.Log("nothing to play");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (_currentGame == null)
			return;

		// 시간으로도 종료 체크
		bool finished = Time.time > _endTime;

		// Tick에서 종료 체크
		if (!finished) {
			finished = _currentGame.Tick();
		}

		if (finished) {
			Debug.Log("game finished");
			_currentGame.Finished();

			// 임금 지급
			MyStatus.instance.money.value += Database<Work>.instance.Find(MyStatus.instance.lastWorkId).payment[0];

			// 돌아가자
			SceneManager.LoadScene("GameScene");
		}

		UpdateGauges();
	}

	void UpdateGauges() {
		scoreGauge.Progress = Mathf.Clamp01(_currentGame.Progress);
		timeGauge.Progress = Mathf.Clamp01((_endTime - Time.time) / _currentGame.MaxTime);
	}
}
