using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectClick : MonoBehaviour, IPointerClickHandler {

    public delegate void OnClickedEvent();

    public event OnClickedEvent OnClicked = delegate{};

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        Debug.Log ("CLick");
        GameObject.Destroy(gameObject);
    }
}
