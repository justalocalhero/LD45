using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour 
{
	private Queue<GameObject> pool;
	public Transform poolContainer;
	public GameObject objectToPool;
	public int amountToPool;

	public void Start()
	{
		pool = new Queue<GameObject>();

		for(int i = 0; i < amountToPool; i++)
		{
			GameObject obj = (GameObject)Instantiate(objectToPool, poolContainer);
			obj.SetActive(false);
			pool.Enqueue(obj);
		}
	}

	public GameObject Get()
	{
		if(pool.Count == 0) return null;
		
		GameObject obj = pool.Dequeue();

		return obj;		
	}

	public void Queue(GameObject obj)
	{
		pool.Enqueue(obj);
	}

	public void Clear()
	{
		pool.Clear();
	}
}
