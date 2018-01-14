using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BoxCollider2D))]
public class TVDefault : MonoBehaviour, IPointerDownHandler {

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
		GameObject.Destroy(gameObject);
    }
}
