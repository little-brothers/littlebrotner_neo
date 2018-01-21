using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TV03 : MonoBehaviour, ISubscribe {
    
    void Start () {
		
	}
	
	void Update () {
		
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

}
