using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour,IManagerInterface
{
    private static OrderManager instance;
    Queue<OrderBoard> orderQueue;

    // Define a delegate
    public delegate void OrderHandler(OrderBoard order);
    public GameObject orderInBubble;
    // Create an event based on the delegate
    public event OrderHandler OnNewOrder;//Chef reference
    private Coroutine bubbleCoroutine;

    public List<Chef> chefs{get;private set;}
    private List<IMachineInterface> machines;
    public float speedMultiplier = 1.0f; // 기본 속도 계수

    
    //private int currentEnabledChef=1;//it will be controled by datamanager
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
        chefs = new List<Chef>();
        Chef[] chefObjects = FindObjectsOfType<Chef>();
        foreach (Chef chef in chefObjects)
        {
            chefs.Add(chef);
        }

        machines = DataManager.Instance.activeMachines;
        MachineListRenew();
    }
    public void SetSpeedMultiplier(float multiplier)
    {
        speedMultiplier = multiplier;
        foreach (Chef chef in chefs)
        {
            chef.MultSpeed(speedMultiplier);
        }
    }
    public void MachineListRenew(){
        machines = DataManager.Instance.activeMachines;
    }
    public void PutOrderInQueue(OrderBoard order)
    {
        orderQueue.Enqueue(order);
        StartCoroutine(TryAssignOrder());
        AppearBubble(15);
    }
    IEnumerator TryAssignOrder()
    {
        while (!isQueueEmpty())
        {
            OrderBoard order = orderQueue.Peek(); // Check the first order in the queue
            IMachineInterface appropriateMachine = FindMachineForOrder(order);
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
    private IMachineInterface FindMachineForOrder(OrderBoard order)
    {
        foreach (var machine in machines)
        {
            if (machine.unlockedFood.foodData.foodName.Equals(order.foodData.foodData.foodName) && machine.IsAvailable)
            {
                return machine;
            }
        }
        return null; // 적합한 기계가 없는 경우
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
    public void AppearBubble(float duration){
        orderInBubble.SetActive(true);
        if (bubbleCoroutine != null)
        {
            StopCoroutine(bubbleCoroutine);
        }
        bubbleCoroutine = StartCoroutine(DisableBubbleAfterTime(duration));
    }
    private IEnumerator DisableBubbleAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        orderInBubble.SetActive(false);
    }
    public void SetData(BusinessData data){
        speedMultiplier=data.chefSpeedMultiplier;
    }
    public void AddDataToStageData(BusinessData data)
    {
         data.chefSpeedMultiplier= speedMultiplier;
    }
}
