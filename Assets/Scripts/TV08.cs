using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class TV08 : MonoBehaviour {
	// moving entity
	struct Person
	{
		public int index;
		public Image image;
		public float activity; // 얼마나 활동적인가?
		public float speed; // 움직이는 속도는?
	}

	[AssetPath("bubbles")]
	struct Bubble : IDatabaseRow
	{
		public int id { get; private set; }
		public string text { get; private set; }
		public string condition1 { get; private set; }
		public string condition2 { get; private set; }

		int IDatabaseRow.ID { get { return this.id; } }

		bool IDatabaseRow.Parse(List<string> row)
		{
			this.id = int.Parse(row[0]);
			this.text = row[1];
			this.condition1 = row[4];
			this.condition2 = row[5];

			return true;
		}
	}

	private List<Image> _people;

	[SerializeField]
	private Sprite[] _sprites;

	[SerializeField]
	private ScriptBubble bubble;

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
		CheckAndShowBubble();
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
		int movePeople = GetThoughtValue();
		AddRandomRaceValue();

		List<Person> shouldMove = new List<Person>();
		for (int i = 0; i < _raceAppearCount.Length; ++i)
		{
			for (int j = 0; j < _raceAppearCount[i]; ++j)
			{
				// default: red fixed position
				int index = GetRandomIndex();
				_people[index].sprite = _sprites[i];
				_people[index].color = Color.red;

				if (movePeople > 0)
				{
					shouldMove.Add(new Person{
						index = index,
						image = _people[index],
						activity = (float)_rand.NextDouble(),
						speed = range(0.1f, 2f),
					});
				}

				movePeople--;
			}
		}

		return shouldMove;
	}

	private void AddRandomRaceValue()
	{
		int availbleCount = 6;
		bool isRobotAppear = MyStatus.Check("V24:YES");
		bool isSnakeAppear = MyStatus.Check("V43:YES");
		_raceAppearCount = new int[] {3, 3, 3, 3, 3, 3, 0, 0};

		if (isRobotAppear)
			_raceAppearCount[6] = 3;
		else
			availbleCount += 3;

		if (isSnakeAppear)
			_raceAppearCount[7] = 3;			
		else
			availbleCount += 3;

		while (availbleCount > 0)
		{
			int index = rangeInt(0, _raceAppearCount.Length);
			if (0 < _raceAppearCount[index] && _raceAppearCount[index] < 5)
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
		person.image.color = Color.HSVToRGB(rangeWith(rand, 0.1f, 0.9f), 0.5f, 1.0f);

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

	void CheckAndShowBubble()
	{
		List<Bubble> bubbles = Database<Bubble>.instance.ToList()
			.Where(b => MyStatus.Check(b.condition1) && MyStatus.Check(b.condition2))
			.ToList();

		if (bubbles.Count != 0)
		{
			int idx = rangeInt(0, 30);
			Vector3 pos = bubble.transform.localPosition;
			pos.x = _people[idx].transform.localPosition.x;
			pos.y = _people[idx].transform.localPosition.y + 16f;

			// update pos
			bubble.transform.localPosition = pos;
			bubble.gameObject.SetActive(true);

			// update text
			Bubble selectedBubble = bubbles[rangeInt(0, bubbles.Count)];
			bubble.setText(selectedBubble.text);
		}
		else
		{
			bubble.gameObject.SetActive(false);
		}
	}
}
