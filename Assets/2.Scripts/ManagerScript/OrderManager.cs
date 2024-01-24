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
    public event OrderHandler OnNewOrder;//Chef reference

    public List<Chef> chefs;
    public List<Machine> machines;
    private int currentEnabledChef=1;//it will be controled by datamanager
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
    void Start()
    {
        //enable chef amount of currentEnabledChef
        for (int i = 0; i < chefs.Count; i++)
        {
            if (i < currentEnabledChef)
            {
                // chef activated
                chefs[i].gameObject.SetActive(true);
            }
            else
            {
                // chef deactivated
                chefs[i].gameObject.SetActive(false);
            }
        }
        MachineListRenew();
    }
    public void MachineListRenew(){
        machines = DataManager.Instance.activeMachines;
    }
    public void PutOrderInQueue(OrderBoard order)
    {
        orderQueue.Enqueue(order);
        StartCoroutine(TryAssignOrder());
    }
    IEnumerator TryAssignOrder()
    {
        while (!isQueueEmpty())
        {
            OrderBoard order = orderQueue.Peek(); // Check the first order in the queue
            Machine appropriateMachine = FindMachineForOrder(order);
            Chef availableChef = FindAvailableChef();

            if (availableChef != null && appropriateMachine != null)
            {
                order = orderQueue.Dequeue(); // Remove the order from the queue
                availableChef.ReceiveOrder(order, appropriateMachine);
                yield break; // Coroutine termination
            }
            else
            {
                orderQueue.Enqueue(orderQueue.Dequeue());
                yield return new WaitForSeconds(1); // Retry after waiting for 1 second
            }
        }
    }


    private Chef FindAvailableChef()
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
    public void ActivateOneChef()
    {
        foreach (Chef chef in chefs)
        {
            // check is chef enabled
            if (!chef.gameObject.activeSelf)
            {
                // if find chef to enable, exit the function
                chef.gameObject.SetActive(true);
                currentEnabledChef++;

                return; 
            }
        }

        // if every server is all activatd

    }

    private Machine FindMachineForOrder(OrderBoard order)
    {
        // Find a machine that matches the order
        foreach (var machine in machines)
        {
            if (machine.unlockedFood.foodData.foodName.Equals(order.foodData.foodData.foodName) && machine.IsAvailable)
            {
                return machine;
            }
        }
        return null; // If there is no suitable machine
    }

   
    public void ChefAvailable()
    {
        // Attempt order allocation when the chef is available
        TryAssignOrder();
    }

    public void MachineAvailable()
    {
        // Attempt order allocation when the machine is available
        TryAssignOrder();
    }
}
