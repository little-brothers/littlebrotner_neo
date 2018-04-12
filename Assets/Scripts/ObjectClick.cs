using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectClick : MonoBehaviour {

    public delegate void OnClickedEvent();

    public event OnClickedEvent OnClicked = delegate{};

    void OnMouseUp()
    {
        // check mouse up occured outside of collider range
        if (GetComponent<Collider2D>().OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
        {
            Debug.Log("clicked?");
            GameObject.Destroy(gameObject);
        }
    }
}
