using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TV08 : MonoBehaviour {

	[SerializeField]
	private Image[] _peoples;

	[SerializeField]
	private Sprite[] _sprites;

	private bool _isRobotAppear = false;
	private bool _isSnakeAppear = false;

	private void Start () 
	{
		ShowRandomPeople();
	}

	private void ShowRandomPeople()
	{
		_isRobotAppear = VoteManager.voteDatas[23].isAgree.Equals(1) ? true : false;
		_isSnakeAppear = VoteManager.voteDatas[42].isAgree.Equals(1) ? true : false;
		for (int i = 0; i < _peoples.Length; ++i)
		{
			
		}
	}
}
