using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragTransform : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    private Vector3 offset;

    public void OnBeginDrag(PointerEventData eventData)
    {
        offset = transform.localPosition - Input.mousePosition;
    }

    public void OnDrag(PointerEventData eventData)
    { 
        if(eventData.dragging) 
        {
            transform.localPosition = (Input.mousePosition + offset);
        }
    }
}
