using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyStatus : MonoBehaviour {

	public const int MaxHealth = 10;

	// singleton
	static MyStatus _instance = null;

	private void Awake()
	{
		if (_instance != null)
		{
			return;
		}

		VoteManager.Initialize("vote");
		AddSleepHook(vote => VoteManager.NextDay());

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
	public delegate void SleepEvent(VoteData voteResult);
	event SleepEvent OnSleep = delegate{};

	public void AddSleepHook(SleepEvent evt)
	{
		OnSleep += evt;
	}

	public void Sleep()
	{
		OnSleep(VoteManager.currentVote);
	}


	/* [-100, 100] 범위를 갖는 성향 */
	public DataUpdateNotifier<int> political = new DataUpdateNotifier<int>(); // [전체주의 - 민주주의]
	public DataUpdateNotifier<int> economy = new DataUpdateNotifier<int>();   // [공산주의 - 자본주의]
	public DataUpdateNotifier<int> mechanic = new DataUpdateNotifier<int>();  // [신토불이 - 산업발전]
	public DataUpdateNotifier<int> health = new DataUpdateNotifier<int>(MaxHealth);
	public DataUpdateNotifier<int> money = new DataUpdateNotifier<int>(); // 돈!
	public DataUpdateNotifier<int> endingIndex = new DataUpdateNotifier<int>(); // 엔딩!
	public DataUpdateNotifier<int> lastWork = new DataUpdateNotifier<int>(-1); // 마지막으로 일한 날짜
	public EventSet technologies = new EventSet(); // 발견한 기술들을 저장
	public Inventory inventory = new Inventory(); // 아이템 목록
	//public DataUpdateNotifier<bool> isRobotAppear = new DataUpdateNotifier<bool>(); // 로봇 종족이 나타났는가
}
