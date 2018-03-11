using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class TV08 : MonoBehaviour {
	// moving entity
	struct Person
	{
		public Image image;
		public float activity; // 얼마나 활동적인가?
		public float speed; // 움직이는 속도는?
	}

	private List<Image> _people;

	[SerializeField]
	private Sprite[] _sprites;

	private bool _isRobotAppear = false;
	private bool _isSnakeAppear = false;
	private int[] _raceAppearCount;
	private System.Random _rand;

	private void Start ()
	{
		// people initialization
		_people = new List<Image>();
		for (int i = 0; i < transform.childCount; ++i)
		{
			var img = transform.GetChild(i).GetComponent<Image>();
			if (img != null)
				_people.Add(img);
		}

		// use instanced random to constant result
		// seed changes by day
		_rand = new System.Random(MyStatus.instance.day);
		var movingPeople = ShowRandomPeople();
		SetupAndPatrolling(movingPeople);
	}

	int rangeIntWith(System.Random rand, int min, int max) {
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

	int rangeInt(int min, int max)
	{
		return rangeIntWith(_rand, min, max);
	}

	float rangeWith(System.Random rand, float min, float max)
	{
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

	float range(float min, float max) {
		return rangeWith(_rand, min, max);
	}

	private List<Person> ShowRandomPeople()
	{
		int currentRace = 0;
		int movePeople = GetThoughtValue();
		AddRandomRaceValue();

		List<Person> shouldMove = new List<Person>();
		for (int i = 0; i < _raceAppearCount.Length; ++i)
		{
			for (int j = 0; j < _raceAppearCount[i]; ++j)
			{
				// default: red fixed position
				int index = GetRandomIndex();
				_people[index].sprite = _sprites[currentRace];
				_people[index].color = Color.red;

				if (movePeople > 0)
				{
					shouldMove.Add(new Person{
						image = _people[index],
						activity = (float)_rand.NextDouble(),
						speed = range(0.1f, 2f),
					});
				}

				movePeople--;
			}

			++currentRace;
		}

		return shouldMove;
	}

	private void AddRandomRaceValue()
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
			int index = rangeInt(0, _raceAppearCount.Length);
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
			int index = rangeInt(0, _people.Count);
			if (_people[index].sprite == null)
				return index;
		}
	}

	private int GetThoughtValue()
	{
		int value = MyStatus.instance.political.value;
		// value = -70;
		
		int multiplyValue = 0;
		if (multiplyValue >= 100)
			multiplyValue = 5;
		else
			multiplyValue = (value / 20) + 1;
			
		return 15 - (3 * multiplyValue);
	}

	private void SetupAndPatrolling(IEnumerable<Person> people)
	{
		foreach(var person in people)
		{
			// make random instance for each people
			StartCoroutine(patrol(person, new System.Random(_rand.Next())));
		}
	}

	IEnumerator patrol(Person person, System.Random rand)
	{
		const float minWait = 0.5f, maxWait = 2f;
		var center = person.image.transform.localPosition;
		System.Func<Vector3> nextPos = () => center + new Vector3(
			rangeWith(rand, -3, 3)*person.activity,
			rangeWith(rand, -5, 5)*person.activity,
			0);

		// initial position
		person.image.transform.localPosition = nextPos();
		person.image.color = Color.HSVToRGB(rangeWith(rand, 0f, 1f), 1, 1);

		// next position to go
		Vector3 targetPos = nextPos();

		float moveStart = Time.time;
		while (true)
		{
			yield return new WaitForSeconds(rangeWith(rand, minWait, maxWait));

			Vector3 left = targetPos - person.image.transform.localPosition;
			float distance = (Time.time - moveStart) * person.speed;
			if (left.sqrMagnitude < distance*distance)
			{
				// update immediately
				person.image.transform.localPosition = targetPos;
				targetPos = nextPos();
				continue;
			}

			// go forward
			person.image.transform.localPosition += left.normalized * distance;
		}
	}
}
