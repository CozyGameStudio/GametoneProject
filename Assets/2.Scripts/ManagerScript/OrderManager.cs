using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    private static OrderManager instance;
    Queue<OrderBoard> orderQueue;

    // Define a delegate
    public delegate void OrderHandler(OrderBoard order);

    // Create an event based on the delegate
    public event OrderHandler OnNewOrder;

    private List<ChefScript> chefs = new List<ChefScript>();

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
        orderQueue = new Queue<OrderBoard>();
        if (null == instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public void RegisterChef(ChefScript chef)
    {
        if (!chefs.Contains(chef))
        {
            chefs.Add(chef);  // add to chefs list
            chef.OnChefAvailable += ChefAvailable;//it will track who's available
        }
    }


    private void ChefAvailable()
    {
        // Assign order to available chef
        if (!isQueueEmpty())
        {
            OrderBoard order = takeOrderInQueue();
            ChefScript availableChef = FindAvailableChef();
            if (availableChef != null)
            {
                availableChef.HandleNewOrder(order);
            }
        }
    }
    public void putOrderInQueue(OrderBoard order)
    {
        ChefScript availableChef = FindAvailableChef();
        //if there's available chef, give order directly
        if (availableChef != null)
        {
            availableChef.HandleNewOrder(order);
            Debug.Log("you cook");
        }
        else//put in queue
        {
            orderQueue.Enqueue(order);
            Debug.Log("no available");
        }
        
    }

    private ChefScript FindAvailableChef()
    {
        // find available chef
        return chefs.Find(chef => chef.IsAvailable);
    }
    public bool isQueueEmpty()
    {
        return (orderQueue.Count == 0) ? true : false;
    }
    public OrderBoard takeOrderInQueue()
    {
        return orderQueue.Dequeue();
    }
    
}
