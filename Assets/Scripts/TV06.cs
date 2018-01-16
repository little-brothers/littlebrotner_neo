using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class TV06 : MonoBehaviour, IPointerDownHandler, ISubscribe {
    
    private void Start () 
	{
		NotifyManager.Subscribe(this);
	}

	void ISubscribe.OnNotifty(object[] values)
    {
        string eventName = values[0] as string;
		
		switch(eventName)
		{
			case EventNames.TurnOffTV:
				NotifyManager.UnSubscribe(this);
				Destroy(this.gameObject);
				break;
		}
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        
    }
}
