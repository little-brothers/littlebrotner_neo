using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct Notification
{
	public static Notification Create(string txt)
	{
		return new Notification { text = txt };
	}

	public string text;
}

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

	public List<Notification> pendingNotis { get; private set; }
	public bool taxPaid { get; private set; }
	public const int MaxHealth = 100;
	const int MaxEnergyHard = 12;
	const int MaxEnergyInit = 9;

	// singleton
	static MyStatus _instance = null;

	private MyStatus()
	{
		if (_instance != null)
			return;

		VoteManager.Initialize();

		// 기본 아이템
		inventory.Put(Item.Create(2));

		// 재화 처리
		AddSleepHook((vote, status, noti) => {
			// 세금
			money.value -= tax;
			taxPaid = money >= 0;

			if (taxPaid)
			{
				var switcher = GameObject.FindObjectOfType<RoomSwitcher>();
				switcher.setRoomIdxImmediate(1);

				// 전기 충전
				MyStatus.instance.energy.value = Mathf.Min(MyStatus.instance.energy + energyCharge, MaxEnergyHard);
			}
			else
			{	
				// 세금을 못냄!
				money.value = 0;
				noti.Add(Notification.Create("I did not manage to pay tax"));
			}
		});

		// 투표 갱신
		AddSleepHook((vote, status, noti) => {
			VoteManager.NextDay();

			string eventName = Database<VoteData>.instance.Find(VoteManager.currentVote.id).eventName.Trim();
			if (eventName == "")
				return;

			// 침략 수준 초기화
			invasion.value = 0;

			switch (eventName) {
			case "sandstorm": homeDestroyed.value = status.day + 1; break;
			case "plague": plague.value = true; break;
			case "invasion_warn": invasion.value = 1; break; 
			case "invasion_alert": invasion.value = 2; break; 
			default:
				Debug.AssertFormat(false, "unhandled event {0}", eventName);
				break;
			}
		});

		// 체력 회복
		AddSleepHook((vote, status, noti) => {
			int recovery = 0;
			if (sick) {
				recovery -= 30;
			}

			if (homeDestroyed != 0) {
				recovery -= 20;
			}

			if(hunger>=3){
				recovery -=50;
			}

			if (inventory.HasItem(5)) {
				recovery += 2;
			}

			if (inventory.HasItem(6)) {
				recovery += 5;
			}

			health.value = Mathf.Clamp(health + recovery, 0, MaxHealth);
		});

		// 배고픔은 항상 증가
		// 음식을 먹으면 -1이 되기 때문에 이 시점에서 다음날 배고픔이 0이 된다
		AddSleepHook((vote, status, noti) => {
			hunger.value++;
		});

		// 기술 확인
		AddSleepHook((vote, status, noti) => {
			var newlyDeveloped = Database<Technology>.instance.ToList()
				.Where(tech => !technologies.Contains(tech.id))
				.Where(tech => Check(tech.condition));

			foreach (var newTech in newlyDeveloped)
			{
				technologies.Put(newTech.id);
				// noti.Add(Notification.Create(string.Format("New technology {0} developed", newTech.name)));
			}
		});
	}

	public static void Reset()
	{
		_instance = null;
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
	public delegate void SleepEvent(Vote voteResult, Snapshot status, List<Notification> notifications);
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
		var notifications = new List<Notification>();

		// execute all sleep hooks
		OnSleep(VoteManager.currentVote, new Snapshot(this), notifications);

		// set notifications
		pendingNotis = notifications;

		// next day!
		day.value++;
		Debug.Log(string.Format("day {0}", day.value));

		// game should be ended?
		int ending = CheckEnding();
		if (ending != 0) {
			endingIndex.value = ending-1;
			SceneManager.LoadScene("EndingScene");
		}
	}

	public List<Notification> GetAndClearNotifications()
	{
		var notis = pendingNotis;
		pendingNotis = null;
		return notis;
	}

	public static bool Check(string condition)
	{
		// 빈 조건은 통과로 간주
		if (condition == "")
			return true;

		// vote
		if (condition[0] == 'V')
		{
			try {
				string[] idAndSelect = condition.Split(':');
				int idx = int.Parse(idAndSelect[0].Substring(1));
				int historyIdx = VoteManager.history.FindIndex(v => v.id == idx);

				if (historyIdx < 0)
					return false;

				if (idAndSelect.Length == 1)
					return true; // 투표를 했는지만 체크

				Vote vote = VoteManager.history[historyIdx];
				switch (vote.selection)
				{
				case VoteSelection.Accept:
					return idAndSelect[1].ToUpper() == "YES";

				case VoteSelection.Decline:
					return idAndSelect[1].ToUpper() == "NO";

				case VoteSelection.Abstention:
					return false;
				}

				Debug.Assert(false, "unknown vote condition " + condition);
				return false;
			} catch (FormatException) {
				// continue
			}
		}

		// technology
		if (condition[0] == 'T')
		{
			try {
				int type = int.Parse(condition.Substring(1));
				return instance.technologies.Contains(type);
			} catch (FormatException) {
				// continue
			}
		}

		if (condition[0] == 'I')
		{
			try {
				int id = int.Parse(condition.Substring(1));
				return instance.inventory.HasItem(id);
			} catch (FormatException) {
				// continue
			}
		}

		if (condition == "sandstorm")
			return instance.homeDestroyed.value == instance.day.value;

		// variable evaluation
		if (condition.StartsWith("Health"))
			return evalExpression(instance.health, condition.Substring("Helath".Length));

		if (condition.StartsWith("Political"))
			return evalExpression(instance.political, condition.Substring("Political".Length));

		if (condition.StartsWith("Money"))
			return evalExpression(instance.money, condition.Substring("Money".Length));

		if (condition.StartsWith("Abstence"))
			return evalExpression(VoteManager.abstentionCount, condition.Substring("Abstence".Length));

		Debug.Assert(false, "unknown condition " + condition);
		return false;
	}

	static bool evalExpression(int leftArg, string exp)
	{
		// Debug.Log(leftArg.ToString() + exp);
		exp = exp.Trim();
		if (exp.StartsWith("==")) 
			return leftArg == int.Parse(exp.Substring(2));

		if (exp.StartsWith("<="))
			return leftArg <= int.Parse(exp.Substring(2));

		if (exp.StartsWith(">="))
			return leftArg >= int.Parse(exp.Substring(2));

		if (exp[0] == '<')
			return leftArg < int.Parse(exp.Substring(1));

		if (exp[0] == '>')
			return leftArg > int.Parse(exp.Substring(1));

		Debug.Assert(false, "unknown expression " + exp);
		return false;
	}

	int CheckEnding()
	{
		foreach (var ending in Database<EndingData>.instance.ToList())
		{
			bool endNow = ending.conditions.All(cond => Check(cond));
			if (endNow)
				return ending.id;
		}

		return 0;
	}

	/* [-100, 100] 범위를 갖는 성향 */
	public DataUpdateNotifier<int> political = new DataUpdateNotifier<int>(); // [전체주의 - 민주주의]
	public DataUpdateNotifier<int> economy = new DataUpdateNotifier<int>();   // [공산주의 - 자본주의]
	public DataUpdateNotifier<int> mechanic = new DataUpdateNotifier<int>();  // [신토불이 - 산업발전]

	// 그 이외
	public DataUpdateNotifier<int> health = new DataUpdateNotifier<int>(MaxHealth); // 건강, 잠잘 때 0이 되면 게임오버
	public DataUpdateNotifier<int> hunger = new DataUpdateNotifier<int>(0); // 배고픔, -1이면 그날 음식을 섭취한 것임
	public DataUpdateNotifier<bool> sick = new DataUpdateNotifier<bool>(false); // 아픈가?
	public DataUpdateNotifier<int> homeDestroyed = new DataUpdateNotifier<int>(); // 집에 문제가 생겼는가? 문제가 생겼으면 생긴 날짜를 적어줌
	public DataUpdateNotifier<bool> plague = new DataUpdateNotifier<bool>(false); // 전염병
	public DataUpdateNotifier<int> invasion = new DataUpdateNotifier<int>(0); // 침략 레벨
	public DataUpdateNotifier<int> day = new DataUpdateNotifier<int>(1); // 현재 날짜
	public DataUpdateNotifier<int> energy = new DataUpdateNotifier<int>(MaxEnergyInit); // TV를 보려면 필요한 자원
	public DataUpdateNotifier<int> energyCharge = new DataUpdateNotifier<int>(4); // 일별 에너지 충전량
	public DataUpdateNotifier<int> money = new DataUpdateNotifier<int>(10); // 돈!
	public DataUpdateNotifier<int> tax = new DataUpdateNotifier<int>(1); // 세금 ㅠㅠ
	public DataUpdateNotifier<int> endingIndex = new DataUpdateNotifier<int>(); // 엔딩!
	public DataUpdateNotifier<int> lastWork = new DataUpdateNotifier<int>(-1); // 마지막으로 일한 날짜
	public EventSet technologies = new EventSet(); // 발견한 기술들을 저장
	public Inventory inventory = new Inventory(); // 아이템 목록
	//public DataUpdateNotifier<bool> isRobotAppear = new DataUpdateNotifier<bool>(); // 로봇 종족이 나타났는가
}
