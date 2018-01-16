using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventNames
{
	public const string TurnOffTV = "TurnOffTV";
	//public const string 
}

public static class NotifyManager {

	private static List<ISubscribe> _subscribers;

	static NotifyManager()
	{
		_subscribers = new List<ISubscribe>();
	}

	public static void Subscribe(ISubscribe subscriber)
	{
		if (IsSubscribing(subscriber))
			return;
		
		_subscribers.Add(subscriber);
	}

	public static void UnSubscribe(ISubscribe subscriber)
	{
		if (!IsSubscribing(subscriber))
			return;

		_subscribers.Remove(subscriber);
	}

	public static bool IsSubscribing(ISubscribe subscriber)
	{
		return _subscribers.Contains(subscriber);
	}

	/// <Summery>
	/// values의 첫번째 값은 string형식의 event name을 넣을 것
	/// 나머지 값은 자유롭게 사용
	/// </Summery>
	public static void Notify(params object[] values)
	{
		object[] parameters = values;

		for (int i = 0; i < _subscribers.Count; ++i)
		{
			_subscribers[i].OnNotifty(parameters);
		}
	}
}
