using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeDragManager : MonoBehaviour
{
    public delegate void OnDrop(VendorNode from, VendorNode to);
    public OnDrop onDrop;

    public Transform nodeContainer;
    private VendorNode[] vendorNodes;
    private VendorNode from;

    void Awake()
    {
        vendorNodes = nodeContainer.GetComponentsInChildren<VendorNode>();

        for(int i = 0; i < vendorNodes.Length; i++)
        {
            vendorNodes[i].index = i;
            RegisterNode(vendorNodes[i]);
        }
    }

    void RegisterNode(VendorNode node)
    {
        node.onDragStart += HandleDragStart;
        node.onDragEnd += HandleDragEnd;
        node.onDrop += HandleDrop;
    }

    void HandleDragStart(VendorNode node)
    {
        from = node;
    }

    void HandleDragEnd(VendorNode node)
    {
        from = null;
    }

    void HandleDrop(VendorNode node)
    {
        if(from == null) return;
        if(node == null) return;
        if(from == node) return;
        if(onDrop != null) onDrop(from, node);
    }
}
