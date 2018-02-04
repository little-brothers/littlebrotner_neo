using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 전반적인 현재 게임 상태(돈, 에너지, 발견 기술 등등...)를 저장하는 곳이다
public class MyStatus {

	// 하루가 지나갈 때 현재 상황을 스냅샷으로 저장해 둔다
	public struct Snapshot {
		public int political;
		public int economy;
		public int mechanic;
		public int health;
		public int money;
		public int energy;
		public int lastWork;
		public int day;

		public Snapshot(MyStatus status)
		{
			this.political = status.political;
			this.economy = status.economy;
			this.mechanic = status.mechanic;
			this.health = status.health;
			this.money = status.money;
			this.energy = status.energy;
			this.lastWork = status.lastWork;
			this.day = status.day;
		}
	}

	public const int MaxHealth = 100;
	const int MaxEnergyHard = 12;
	const int MaxEnergyInit = 4;
	int _energyCharge = MaxEnergyInit; // 하룻밤마다 충전되는 에너지량

	// singleton
	static MyStatus _instance = null;

	private MyStatus()
	{
		if (_instance != null)
			return;

		VoteManager.Initialize("vote");

		// 매일 밤마다 전기 충전
		AddSleepHook((vote, status) => MyStatus.instance.energy.value = Mathf.Min(MyStatus.instance.energy + _energyCharge, MaxEnergyHard));

		// 체력 회복
		AddSleepHook((vote, status) => {
			int recovery = 0;
			if (sick) {
				recovery -= 30;
			}

			if (inventory.HasItem(5)) {
				recovery += 2;
			}

			if (inventory.HasItem(6)) {
				recovery += 5;
			}

			health.value = Mathf.Clamp(health + recovery, 0, MaxHealth);
		});
	}

	public static MyStatus instance
	{
		get {
			if (_instance == null)
				_instance = new MyStatus();

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
		public delegate void DataUpdatedEvent(Item item);
		public event DataUpdatedEvent OnUpdate = delegate{};
		public const int MaxSize = 8;

		HashSet<int> _installed = new HashSet<int>(); // 설치형이 들어가논곳
		List<int> _slot = new List<int>(); // 소비형이 들어가는곳

		public List<int> slot {get{ return _slot; }}

		// 아이템 사용
		public bool Use(Item item)
		{
			if (item.installable)
				return false;

			if (!_slot.Contains(item.id))
				return false;

			_slot.Remove(item.id);
			OnUpdate(item);
			return true;
		}

		public bool Put(Item item)
		{
			if (item.installable) {
				if (!_installed.Contains(item.id))
					_installed.Add(item.id);

			} else if (_slot.Count >= MaxSize) {
				return false;

			} else {
				_slot.Add(item.id);
			}

			OnUpdate(item);
			return true;
		}

		public bool HasItem(int id)
		{
			return _installed.Contains(id) || _slot.Contains(id);
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

	public void ResetAllHooks()
	{
		OnSleep = null;
	}

	public void Sleep()
	{
		// execute all sleep hooks
		OnSleep(VoteManager.currentVote, new Snapshot(this));

		// next day!
		day.value++;

		// 투표는 hook하지 않고 별도로 처리
		// 엔딩 체크를 위해
		VoteManager.NextDay();

	}

	public static bool Check(string condition)
	{
		// 빈 조건은 통과로 간주
		if (condition == "")
			return true;

		switch (condition[0])
		{
		case 'V':
			string[] idAndSelect = condition.Split(':');
			int idx = int.Parse(idAndSelect[0].Substring(1));
			VoteData vote = VoteManager.voteDatas[idx];

			if (vote.choice == VoteSelection.NotYet)
				return false;

			if (idAndSelect.Length == 1)
				return true; // 투표를 했는지만 체크

			switch (vote.choice)
			{
			case VoteSelection.Accept:
				return idAndSelect[1].ToUpper() == "YES";

			case VoteSelection.Decline:
				return idAndSelect[1].ToUpper() == "NO";
			}

			Debug.Assert(false, "unknown vote condition " + condition);
			return false;

		case 'T':
			int type = int.Parse(condition.Substring(1));
			return instance.technologies.Contains(type);
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
	public DataUpdateNotifier<bool> sick = new DataUpdateNotifier<bool>(false); // 아픈가?
	public DataUpdateNotifier<int> day = new DataUpdateNotifier<int>(1); // 현재 날짜
	public DataUpdateNotifier<int> energy = new DataUpdateNotifier<int>(MaxEnergyInit); // TV를 보려면 필요한 자원
	public DataUpdateNotifier<int> money = new DataUpdateNotifier<int>(); // 돈!
	public DataUpdateNotifier<int> tax = new DataUpdateNotifier<int>(); // 세금 ㅠㅠ
	public DataUpdateNotifier<int> endingIndex = new DataUpdateNotifier<int>(); // 엔딩!
	public DataUpdateNotifier<int> lastWork = new DataUpdateNotifier<int>(-1); // 마지막으로 일한 날짜
	public EventSet technologies = new EventSet(); // 발견한 기술들을 저장
	public Inventory inventory = new Inventory(); // 아이템 목록
	//public DataUpdateNotifier<bool> isRobotAppear = new DataUpdateNotifier<bool>(); // 로봇 종족이 나타났는가
}
