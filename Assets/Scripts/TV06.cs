using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TV06 : MonoBehaviour, ISubscribe, IPointerDownHandler, IPointerUpHandler {
    
	[SerializeField]
	private float _closeSectionSize = 0f;

	private Vector3 _touchOffset = Vector3.zero;

	[SerializeField]
	private VerticalLayoutGroup list;

	private void Start () 
	{
		int twitID = VoteManager.currentVote.id;
		TwitPost todayTwit = Database<TwitPost>.instance.Find(twitID);
		List<TwitListElement.Data> twits = new List<TwitListElement.Data>();

		if (todayTwit.LB.Length != 0)       twits.Add(new TwitListElement.Data(twitID, "lb"));
		if (todayTwit.pigeon.Length != 0)   twits.Add(new TwitListElement.Data(twitID, "pigeon"));
		if (todayTwit.mantis.Length != 0)   twits.Add(new TwitListElement.Data(twitID, "mantis"));
		if (todayTwit.cat.Length != 0)      twits.Add(new TwitListElement.Data(twitID, "cat"));
		if (todayTwit.elephant.Length != 0) twits.Add(new TwitListElement.Data(twitID, "elephant"));
		if (todayTwit.frog.Length != 0)     twits.Add(new TwitListElement.Data(twitID, "frog"));
		if (todayTwit.robot.Length != 0)    twits.Add(new TwitListElement.Data(twitID, "robot"));
		if (todayTwit.snake.Length != 0)    twits.Add(new TwitListElement.Data(twitID, "snake"));

		// shuffle array
		var rand = new System.Random(VoteManager.currentVote.id);
		for (int i = 2; i < twits.Count; ++i)
		{
			int r = rand.Next(i);
			if (r == i)
				continue;

			var temp = twits[r];
			twits[r] = twits[i];
			twits[i] = temp;
		}

		var elemTemplate = Resources.Load<GameObject>("TwitBox");
		foreach (var twit in twits)
		{
			var elem = GameObject.Instantiate(elemTemplate, list.transform).GetComponent<TwitListElement>();
			elem.singleTwit = twit;
		}
	}

	void ISubscribe.OnNotifty(object[] values)
    {
        string eventName = values[0] as string;
		
		switch(eventName)
		{
			case EventNames.TurnOffTV:
				Destroy(this.gameObject);
				break;
		}
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
		// _touchOffset = Vector3.zero;
		// _touchOffset = Input.mousePosition;
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        // _touchOffset -= Input.mousePosition;

		// if (Mathf.Abs(_touchOffset.x) <= _closeSectionSize &&
		// 	Mathf.Abs(_touchOffset.y) <= _closeSectionSize)
		// 	GameObject.Destroy(gameObject);
    }
}
