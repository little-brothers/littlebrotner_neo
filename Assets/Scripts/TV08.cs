using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
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

	int rangeInt(System.Random rand, int min, int max) {
		if (max < min)
		{
			int temp = min;
			min = max;
			max = temp;
		}

		int size = max - min;
		if (size == 0)
			return min;

		return (int)(rand.NextDouble()*(double)size + (double)min);
	}

	float range(System.Random rand, float min, float max) {
		if (max < min)
		{
			float temp = min;
			min = max;
			max = temp;
		}

		float size = max - min;
		if (size < float.Epsilon)
			return min;

		float value = (float)rand.NextDouble();
		return value*size + min;
	}

	private void ShowRandomPeople()
	{
		// use instanced random to constant result
		// seed changes by day
		var rand = new System.Random(MyStatus.instance.day);
		int currentRace = 0;
		AddRandomRaceValue(rand);
		SetThoughtValue();
		for (int i = 0; i < _raceAppearCount.Length; ++i)
		{
			for (int j = 0; j < _raceAppearCount[i]; ++j)
			{
				int index = GetRandomIndex(rand);
				_peoples[index].sprite = _sprites[currentRace];
				_peoples[index].color = Color.red;
				if (_movePeoples > 0)
				{
					Vector3 pos = _peoples[index].transform.localPosition;
					float x = range(rand, -3, 4);
					float y = range(rand, -5, 6);
					pos.x += x; pos.y += y;
					_peoples[index].color = Color.HSVToRGB(range(rand, 0f, 1f), 1, 1);
					_peoples[index].transform.localPosition = pos;
					--_movePeoples;
				}
			}
			++currentRace;
		}
	}

	private void AddRandomRaceValue(System.Random rand)
	{
		int availbleCount = 6;
		_isRobotAppear = MyStatus.Check("V24:YES");
		_isSnakeAppear = MyStatus.Check("V43:YES");
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
			int index = rangeInt(rand, 0, _raceAppearCount.Length);
			if (_raceAppearCount[index] < 5 && !_raceAppearCount[index].Equals(0))
			{
				--availbleCount;
				_raceAppearCount[index] += 1;
			}
		}
	}

	private int GetRandomIndex(System.Random rand)
	{
		while (true)
		{
			int index = rangeInt(rand, 0, _peoples.Length);
			if (_peoples[index].sprite == null)
				return index;
		}
	}

	private void SetThoughtValue()
	{
		int value = MyStatus.instance.political.value;
		// value = -70;
		
		int multiplyValue = 0;
		if (multiplyValue >= 100)
			multiplyValue = 5;
		else
			multiplyValue = (value / 20) + 1;
			
		_movePeoples = 15 - (3 * multiplyValue);
	}
}
