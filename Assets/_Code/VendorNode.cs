using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VendorNode : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDropHandler, IDragHandler
{
    public int index;
    new public string name;
    public List<Resource> demands;
    public List<Resource> sells;

    public delegate void StartDrag(VendorNode node);
    public StartDrag onDragStart;

    public delegate void EndDrag(VendorNode node);
    public EndDrag onDragEnd;

    public delegate void Drop(VendorNode node);
    public Drop onDrop;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(onDragStart != null) onDragStart(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(onDrop != null) onDrop(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(onDragEnd != null) onDragEnd(this);
    }

    public float GetBuyFactor(Resource resource)
    {
        foreach(Resource demand in demands)
        {
            if(resource == demand) return 2;
        }

        foreach(Resource sell in sells)
        {
            if(resource == sell) return .5f;
        }

        return 1.1f;
    }
}
