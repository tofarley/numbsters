// 2024-09-15 AI-Tag 
// This was created with assistance from Muse, a Unity Artificial Intelligence product

using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDropSwap : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 startPosition;
    private Transform startParent;

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = transform.position;
        startParent = transform.parent;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = startPosition;
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        if (eventData.pointerEnter != null && eventData.pointerEnter.transform.parent == startParent)
        {
            Transform otherObject = eventData.pointerEnter.transform;
            otherObject.position = startPosition;
            transform.position = otherObject.position;
        }
        else
        {
            transform.position = startPosition;
        }
    }
}
