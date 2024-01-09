using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    private static OrderManager instance;
    Queue<string> orderQueue;
    public static OrderManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    public void Awake()
    {
        orderQueue = new Queue<string>();
        if (null == instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void putOrderInQueue(string order)
    {
        orderQueue.Enqueue(order);
        Debug.Log(order);
    }
    public bool isQueueEmpty()
    {
        return (orderQueue.Count == 0) ? true : false;
    }
    public string takeOrderInQueue()
    {
        return orderQueue.Dequeue();
    }
}
