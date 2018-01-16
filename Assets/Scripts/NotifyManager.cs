using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NotifyManager {

	private static List<ISubscribe> _subscribers;

	static NotifyManager()
	{
		_subscribers = new List<ISubscribe>();
	}

	static void Subscribe(ISubscribe subscriber)
	{
		_subscribers.Add(subscriber);
	}

	static void UnSubscribe(ISubscribe subscriber)
	{
		_subscribers.Remove(subscriber);
	}

	/// <Summery>
	/// values의 첫번째 값은 string형식의 event name을 넣을 것
	/// 나머지 값은 자유롭게 사용
	/// </Summery>
	static void Notify(params object[] values)
	{
		object[] parameters = values;

		foreach (ISubscribe subscriber in _subscribers)
		{
			subscriber.GetMessage(parameters);
		}
	}
}
