using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 전반적인 현재 게임 상태(돈, 에너지, 발견 기술 등등...)를 저장하는 곳이다
public class MyStatus : MonoBehaviour {

	// 하루가 지나갈 때 현재 상황을 스냅샷으로 저장해 둔다
	public struct Snapshot {
		public int political;
		public int economy;
		public int mechanic;
		public int health;
		public int money;
		public int energy;
		public int lastWork;

		public Snapshot(MyStatus status)
		{
			this.political = status.political;
			this.economy = status.economy;
			this.mechanic = status.mechanic;
			this.health = status.health;
			this.money = status.money;
			this.energy = status.energy;
			this.lastWork = status.lastWork;
		}
	}

	public const int MaxHealth = 10;
	const int MaxEnergyHard = 12;
	const int MaxEnergyInit = 4;
	int _energyCharge = MaxEnergyInit; // 하룻밤마다 충전되는 에너지량

	// singleton
	static MyStatus _instance = null;

	private void Awake()
	{
		if (_instance != null)
		{
			return;
		}

		VoteManager.Initialize("vote");

		// 투표 다음날 처리
		AddSleepHook((vote, status) => VoteManager.NextDay());

		// 매일 밤마다 에너지 충전
		AddSleepHook((vote, status) => MyStatus.instance.energy.value = Mathf.Min(MyStatus.instance.energy + _energyCharge, MaxEnergyHard));

		DontDestroyOnLoad(gameObject);
	}

	public static MyStatus instance
	{
		get {
			if (_instance == null)
				_instance = GameObject.FindObjectOfType<MyStatus>();

			Debug.Assert(_instance != null);
			return _instance;
		}
	}

	public class EventSet
	{
		public delegate void DataUpdatedEvent(int eventType);
		public event DataUpdatedEvent OnUpdate = delegate{};

		HashSet<int> _events = new HashSet<int>();

		public void Put(int eventType)
		{
			if (_events.Contains(eventType))
				return;

			_events.Add(eventType);
			OnUpdate(eventType);
		}

		public bool Contains(int type)
		{
			return _events.Contains(type);
		}
	}

	public class Inventory
	{
		public delegate void DataUpdatedEvent(int item, int count);
		public event DataUpdatedEvent OnUpdate = delegate{};

		Dictionary<int, int> _map;

		public void Put(int item, int count)
		{
			if (_map.ContainsKey(item)) {
				_map[item] += count;
			} else {
				_map.Add(item, count);
			}

			OnUpdate(item, _map[item]);
		}

		public int GetItem(int item)
		{
			if (_map.ContainsKey(item))
				return _map[item];

			return 0;
		}
	}

	public class DataUpdateNotifier<T>
	{
		public delegate void DataUpdatedEvent(T value);
		public event DataUpdatedEvent OnUpdate = delegate{};
		public static implicit operator T(DataUpdateNotifier<T> value)
		{
			return value.value;
		}

		public DataUpdateNotifier(T initial = default(T))
		{
			_value = initial;
		}

		T _value;

		public T value {
			get { return _value; }
			set { _value = value; OnUpdate(value); }
		}
	}

	// hooks for sleep
	public delegate void SleepEvent(VoteData voteResult, Snapshot status);
	event SleepEvent OnSleep = delegate{};

	public void AddSleepHook(SleepEvent evt)
	{
		OnSleep -= evt; // 혹시나 설마 없겠지만 만약에 실수로 중복 등록하는 경우를 위한 대비
		OnSleep += evt;
	}

	public void Sleep()
	{
		OnSleep(VoteManager.currentVote, new Snapshot(this));
	}

	public static bool Check(string condition)
	{
		// 빈 조건은 통과로 간주
		if (condition == "")
			return true;

		switch (condition[0])
		{
		case 'V':
			return true;

		case 'T':
			return true;
		}

		Debug.Assert(false, "unknown condition " + condition);
		return false;
	}


	/* [-100, 100] 범위를 갖는 성향 */
	public DataUpdateNotifier<int> political = new DataUpdateNotifier<int>(); // [전체주의 - 민주주의]
	public DataUpdateNotifier<int> economy = new DataUpdateNotifier<int>();   // [공산주의 - 자본주의]
	public DataUpdateNotifier<int> mechanic = new DataUpdateNotifier<int>();  // [신토불이 - 산업발전]

	// 그 이외
	public DataUpdateNotifier<int> health = new DataUpdateNotifier<int>(MaxHealth); // 건강, 잠잘 때 0이 되면 게임오버
	public DataUpdateNotifier<int> energy = new DataUpdateNotifier<int>(MaxEnergyInit); // TV를 보려면 필요한 자원
	public DataUpdateNotifier<int> money = new DataUpdateNotifier<int>(); // 돈!
	public DataUpdateNotifier<int> endingIndex = new DataUpdateNotifier<int>(); // 엔딩!
	public DataUpdateNotifier<int> lastWork = new DataUpdateNotifier<int>(-1); // 마지막으로 일한 날짜
	public EventSet technologies = new EventSet(); // 발견한 기술들을 저장
	public Inventory inventory = new Inventory(); // 아이템 목록
	//public DataUpdateNotifier<bool> isRobotAppear = new DataUpdateNotifier<bool>(); // 로봇 종족이 나타났는가
}
