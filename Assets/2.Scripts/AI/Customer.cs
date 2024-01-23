using UnityEngine;
using MonsterLove.StateMachine;
using UnityEngine.AI;

public struct OrderBoard{
    public Food foodData{get;private set;}
    public int tableNumber { get; private set;}
    public OrderBoard(Food food, int num){
        foodData=food;
        tableNumber=num;
    }
}
public class Customer : MonoBehaviour
{ 
    public enum States { 
        Idle,
        Walk,
        Order
    }

    StateMachine<States, StateDriverUnity> fsm;
    public GameObject orderBubble;
    public SpriteRenderer foodRenderer;
    public Transform foodHolder;
    [Header("Character")]
    public float speed = 5f;

    private GameObject customerTablePlace;
    private GameObject customerBackPlace;
    private int customerChairPlaceLength;
    private Food orderFood;
    private bool isOrdered = false;
    public int tableNumber{get;private set;}
    private bool receiveOrder=false;
    private NavMeshAgent agent;

    private void Awake()
    {
        fsm = new StateMachine<States, StateDriverUnity>(this);
        fsm.ChangeState(States.Idle);
        orderBubble.SetActive(false);
    }
    void Start(){
        orderFood = DataManager.Instance.RandomFood();
        Debug.Log(orderFood.foodData.foodName);
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
    }
    private void Update()
    {
        fsm.Driver.Update.Invoke();
    }
    void Idle_Enter()
    {
        
    }
    void Idle_Update() 
    {
        // If no order is placed, receive the table location for a move call
        if (!isOrdered)
        {
            
            if (!CustomerManager.Instance.IsCustomerFull())
            {
                customerChairPlaceLength = CustomerManager.Instance.customerChair.Count;
                for (int i = 0; i < customerChairPlaceLength; i++)
                {
                    if (!CustomerManager.Instance.customerTablePresent[i])
                    {
                        tableNumber = i+1;
                        customerTablePlace = CustomerManager.Instance.customerChair[i];
                        CustomerManager.Instance.customerTablePresent[i] = true;
                        break;
                    }
                }
                fsm.ChangeState(States.Walk);

            }
        }
        // If an order is placed, add money and receive the return location for a move call 
        else
        {
            GameManager.Instance.AddMoney(orderFood.currentValue);
            StageMissionManager.Instance.IncreaseAccumulatedCustomer();
            StageMissionManager.Instance.IncreaseAccumulatedSales(orderFood.currentValue);
            Debug.Log(orderFood.currentValue);
            customerBackPlace = CustomerManager.Instance.customerBackPlace;
            fsm.ChangeState(States.Walk);
        }
    }

    void Walk_Update()
    {
        // Move to the table if no order is placed
        if (!isOrdered)
        {
            if (Vector2.Distance(transform.position, customerTablePlace.transform.position) > .3f)
            {
                agent.SetDestination(customerTablePlace.transform.position);
            }
            else
            {
                fsm.ChangeState(States.Order);
            }
        }
        // Move to the return location if an order is placed
        else
        {
            if (Vector2.Distance(transform.position, customerBackPlace.transform.position) > 0.3f)
            {
                agent.SetDestination(customerBackPlace.transform.position);
            }
            else
            {
                CustomerManager.Instance.customerTablePresent[tableNumber-1] = false;
                
                Destroy(gameObject);
            }
            //hey
        }
    }
    void Walk_Exit()
    {
        
    }

    void Order_Enter()
    {
        transform.SetParent(customerTablePlace.transform);
        
        isOrdered = true;
        OrderBoard newOrder=new OrderBoard(orderFood,tableNumber);
        OrderManager.Instance.PutOrderInQueue(newOrder);
        orderBubble.SetActive(true);
        foodRenderer.sprite=orderFood.foodData.foodIcon;
;    }
    void Order_Update()
    {
        if(receiveOrder)
        {
            fsm.ChangeState(States.Idle);
        }
    }

    void Order_Exit()
    {
        transform.SetParent(null);
        orderBubble.SetActive(false);
        
    }
    public void GetMenu(GameObject menu)
    {
        menu.transform.SetParent(foodHolder);
        menu.transform.position=foodHolder.position;
        receiveOrder = true;
        
    }

}
