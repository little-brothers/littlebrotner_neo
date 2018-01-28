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
	private int _movePeoples;
	private int[] _raceAppearCount;

	private void Start ()
	{
		ShowRandomPeople();
	}

	private void ShowRandomPeople()
	{
		int currentRace = 0;
		AddRandomRaceValue();
		SetThoughtValue();
		for (int i = 0; i < _raceAppearCount.Length; ++i)
		{
			for (int j = 0; j < _raceAppearCount[i]; ++j)
			{
				int index = GetRandomIndex();
				_peoples[index].sprite = _sprites[currentRace];
				_peoples[index].color = Color.red;
				if (_movePeoples > 0)
				{
					float x = Random.Range(-86, 86);
					float y = Random.Range(-36, 36);
					_peoples[index].color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f, 1f, 1f);
					_peoples[index].transform.localPosition = new Vector3(x, y, _peoples[index].transform.position.z);
					--_movePeoples;
				}
			}
			++currentRace;
		}
	}

	private void AddRandomRaceValue()
	{
		int availbleCount = 6;
		_isRobotAppear = VoteManager.voteDatas[23].isAgree.Equals(1) ? true : false;
		_isSnakeAppear = VoteManager.voteDatas[42].isAgree.Equals(1) ? true : false;
		_raceAppearCount = new int[] {3, 3, 3, 3, 3, 3, 0, 0};

		if (_isRobotAppear)
			_raceAppearCount[6] = 3;
		else
			availbleCount += 3;

		if (_isSnakeAppear)
			_raceAppearCount[7] = 3;			
		else
			availbleCount += 3;

		while (availbleCount > 0)
		{
			int index = Random.Range(0, _raceAppearCount.Length);
			if (_raceAppearCount[index] < 5 && !_raceAppearCount[index].Equals(0))
			{
				--availbleCount;
				_raceAppearCount[index] += 1;
			}
		}
	}

	private int GetRandomIndex()
	{
		while (true)
		{
			int index = Random.Range(0, _peoples.Length);
			if (_peoples[index].sprite == null)
				return index;
		}
	}

	private void SetThoughtValue()
	{
		int value = MyStatus.instance.political.value;
		value = -70;
		
		int multiplyValue = 0;
		if (multiplyValue >= 100)
			multiplyValue = 5;
		else
			multiplyValue = (value / 20) + 1;
			
		_movePeoples = 15 - (3 * multiplyValue);
	}
}
