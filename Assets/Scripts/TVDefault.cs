using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BoxCollider2D))]
public class TVDefault : MonoBehaviour, IPointerDownHandler, ISubscribe {

    private void Start()
    {
      NotifyManager.Subscribe(this);
    }

    void ISubscribe.OnNotifty(object[] values)
    {
      string eventName = values[0] as string;
		
		  switch(eventName)
		  {
			  case EventNames.TurnOffTV:
				  Destroy(gameObject);
				  break;
		  }
    }

    void OnDestroy()
	  {
		  NotifyManager.UnSubscribe(this);
	  }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
	    GameObject.Destroy(gameObject);
    }
}
