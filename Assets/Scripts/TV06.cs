using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class TV06 : MonoBehaviour, ISubscribe, IPointerDownHandler, IPointerUpHandler {
    
	[SerializeField]
	private float _closeSectionSize = 0f;

	private Vector3 _touchOffset = Vector3.zero;

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
