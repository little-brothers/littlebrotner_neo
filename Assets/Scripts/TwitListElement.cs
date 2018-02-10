using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AssetPath("twit")]
public struct TwitPost : IDatabaseRow
{
	public int id;
	public string LB;
	public string pigeon;
	public string mantis;
	public string cat;
	public string elephant;
	public string frog;
	public string robot;
	public string snake;

    int IDatabaseRow.ID { get { return id; } }

    bool IDatabaseRow.Parse(List<string> row)
    {
		id = int.Parse(row[0].Substring(1));
		LB = row[4];
		pigeon = row[5];
		mantis = row[6];
		cat = row[7];
		elephant = row[8];
		frog = row[9];
		robot = row[10];
		snake = row[11];

		return true;
    }
}

public class TwitListElement : MonoBehaviour {
	public struct Data {
		public Data(int id, string speicies)
		{
			this.id = id;
			this.speicies = speicies;
		}

		public int id;
		public string speicies;
	}

	private Image _profile;
	private Text _twit;
	private LikeButton _likeButton;

	[SerializeField]
	private List<Sprite> icons = new List<Sprite>();

	void Awake()
	{
		_profile = transform.Find("Profile").GetComponent<Image>();
		_twit = transform.Find("Text").GetComponent<Text>();
		_likeButton = transform.Find("Like").GetComponent<LikeButton>();
	}

	public Data singleTwit {
		set {
			TwitPost twit = Database<TwitPost>.instance.Find(value.id);
			switch (value.speicies)
			{
			case "lb":
				_twit.text = twit.LB;
				_profile.sprite = icons[0];
				break;

			case "pigeon":
				_twit.text = twit.pigeon;
				_profile.sprite = icons[1];
				break;

			case "mantis":
				_twit.text = twit.mantis;
				_profile.sprite = icons[2];
				break;

			case "cat":
				_twit.text = twit.cat;
				_profile.sprite = icons[3];
				break;

			case "elephant":
				_twit.text = twit.elephant;
				_profile.sprite = icons[4];
				break;

			case "frog":
				_twit.text = twit.frog;
				_profile.sprite = icons[5];
				break;

			case "robot":
				_twit.text = twit.robot;
				_profile.sprite = icons[6];
				break;

			case "snake":
				_twit.text = twit.snake;
				_profile.sprite = icons[7];
				break;
			}
		}
	}
}
