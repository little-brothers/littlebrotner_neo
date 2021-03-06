﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MineGame : MonoBehaviour, IMinigame {
	const int MaxScore = 50;
	const float GameTime = 10f;
	int _score;


	[SerializeField]
	AudioSource soundFx;

	[SerializeField]
	AudioClip click;

	[SerializeField]
	AudioClip hover;


	public void HoverSound(){
		soundFx.PlayOneShot (hover);
	}

	public void ClickSound(){
		soundFx.PlayOneShot (click);
	}
		




	[SerializeField]
	Button mine;

    float IMinigame.Progress {
		get { return _score / (float)MaxScore; }
	}

    float IMinigame.MaxTime {
		get { return GameTime; }
	}

    void IMinigame.Finished()
    {
		mine.onClick.RemoveAllListeners();
    }

    void IMinigame.Setup()
    {
		_score = 0;
		mine.onClick.AddListener(() => ++_score);
    }

    bool IMinigame.Tick()
    {
		return false;
    }
}
