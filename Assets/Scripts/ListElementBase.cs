
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ListElementBase : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler {
    void Start() {
        GetComponent<CanvasRenderer>().SetAlpha(0);
    }

	void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
	{
		GetComponent<CanvasRenderer>().SetAlpha(0);
	}

	void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
	{
		GetComponent<CanvasRenderer>().SetAlpha(.1f);
	}
}