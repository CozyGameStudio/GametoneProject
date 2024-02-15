using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour,IBusinessManagerInterface
{
    private static OrderManager instance;
    Queue<OrderBoard> orderQueue=new Queue<OrderBoard>();

    // Define a delegate
    public delegate void OrderHandler(OrderBoard order);
    public GameObject orderInBubble;
    // Create an event based on the delegate
    public event OrderHandler OnNewOrder;//Chef reference
    private Coroutine bubbleCoroutine;

    public List<Chef> chefs{get;private set;}
    private List<IMachineInterface> machines;
    public float speedMultiplier = 1.0f; // 기본 속도 계수
    public bool isRewardActivated { get; private set; }
    public bool isOrderedForTutorial = false;
    public delegate void RewardTimeCheckDelegate(float timeLeft);
    public event RewardTimeCheckDelegate OnRewardTimeCheckDelegate;
    public static OrderManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<OrderManager>();
            }
            return instance;
        }
    }
    public void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        chefs = new List<Chef>();
        Chef[] chefObjects = FindObjectsOfType<Chef>();
        foreach (Chef chef in chefObjects)
        {
            chefs.Add(chef);
            chef.SetSpeed(speedMultiplier);
        }
        
        machines = DataManager.Instance.activeMachines;
        MachineListRenew();
    }
    public void MachineListRenew(){
        machines = DataManager.Instance.activeMachines;
    }
    public void PutOrderInQueue(OrderBoard order)
    {
        orderQueue.Enqueue(order);
        StartCoroutine(TryAssignOrder());
        AppearBubble(2);
    }
    IEnumerator TryAssignOrder()
    {
        while (!isQueueEmpty())
        {
            isOrderedForTutorial = true;

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
        if(orderInBubble!=null)orderInBubble.SetActive(true);
        if (bubbleCoroutine != null)
        {
            StopCoroutine(bubbleCoroutine);
        }
        bubbleCoroutine = StartCoroutine(DisableBubbleAfterTime(duration));
    }
    private IEnumerator DisableBubbleAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        if (orderInBubble != null) orderInBubble.SetActive(false);
    }
   
    public IEnumerator SetIsRewardActivated(float time)
    {
        isRewardActivated = true;
        float timeLeft = time;
        while (timeLeft > 0)
        {
            OnRewardTimeCheckDelegate?.Invoke(timeLeft); 
            yield return new WaitForSeconds(1f);
            timeLeft -= 1f; 
        }
        OnRewardTimeCheckDelegate?.Invoke(0); 
        isRewardActivated = false;
    }
    public void SetData(BusinessData data){
        speedMultiplier=data.chefSpeedMultiplier;
    }
    public void AddDataToBusinessData(BusinessData data)
    {
         data.chefSpeedMultiplier= speedMultiplier;
    }
}
