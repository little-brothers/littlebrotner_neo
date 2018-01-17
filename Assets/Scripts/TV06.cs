using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class TV06 : MonoBehaviour, IPointerDownHandler, ISubscribe {
    
    private void Start () 
	{
		NotifyManager.Subscribe(this);
		Utilities.SetUIParentFit(GameObject.FindGameObjectWithTag("RootCanvas"), gameObject);
	}

	void OnDestroy()
	{
		NotifyManager.UnSubscribe(this);
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
        
    }
}
