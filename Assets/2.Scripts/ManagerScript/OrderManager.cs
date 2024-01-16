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
    public event OrderHandler OnNewOrder;//chefScript reference

    public List<ChefScript> chefs = new List<ChefScript>();
    public Machine[] machines;
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
            OrderBoard order = orderQueue.Peek(); // 큐의 첫 번째 주문 확인
            Machine appropriateMachine = FindMachineForOrder(order);
            ChefScript availableChef = FindAvailableChef();

            if (availableChef != null && appropriateMachine != null)
            {
                order = orderQueue.Dequeue(); // 큐에서 주문 제거
                availableChef.ReceiveOrder(order, appropriateMachine);
                yield break; // Coroutine 종료
            }
            else
            {
                orderQueue.Enqueue(orderQueue.Dequeue());
                yield return new WaitForSeconds(1); // 1초 동안 기다린 후 다시 시도
            }
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
    public void ActivateOneChef()
    {
        foreach (ChefScript chef in chefs)
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
    public void AddOrder(OrderBoard order)
    {
        // 새 오더를 대기열에 추가
        orderQueue.Enqueue(order);

        // 주문 할당 시도
        TryAssignOrder();
    }

    private Machine FindMachineForOrder(OrderBoard order)
    {
        // 주문에 맞는 기계 찾기
        foreach (var machine in machines)
        {
            if (machine.foodToMake.Equals(order.name) && machine.IsAvailable)
            {
                return machine;
            }
        }
        return null; // 적절한 기계가 없는 경우
    }

   
    public void ChefAvailable()
    {
        // 셰프가 사용 가능할 때 주문 할당 시도
        TryAssignOrder();
    }

    public void MachineAvailable()
    {
        // 머신이 사용 가능할 때 주문 할당 시도
        TryAssignOrder();
    }
}
