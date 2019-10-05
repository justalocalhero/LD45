using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerDispatch : MonoBehaviour
{
    private CoinPurse coinPurse;
    public ObjectPool walkerPool;
    public NodeDragManager nodeDragManager;

    void Awake()
    {
        nodeDragManager.onDrop += Dispatch;
    }

    void Start()
    {
        coinPurse = CoinPurse.instance;
    }

    private void Dispatch(VendorNode from, VendorNode to)
    {
        Walker walker = GetWalker();
        if(walker == null) return;

        walker.transform.position = from.transform.position;

        walker.toNode = to;
        walker.fromNode = from;
        walker.coins = coinPurse.coins;
        coinPurse.Adjust(-coinPurse.coins);
        walker.Negotiate();

    }

    private void Kill(Walker walker)
    {
        walker.onKill -= Kill;
        walker.gameObject.SetActive(false);   
        walkerPool.Queue(walker.gameObject);
    }

    private Walker GetWalker()
    {
        GameObject obj = walkerPool.Get();

        if(obj == null) 
        {
            Debug.LogWarning("EmptyPool");
            return null;
        }

        obj.SetActive(true);

        Walker walker = obj.GetComponent<Walker>();

        walker.onKill += Kill;

        return walker;
    }
}
