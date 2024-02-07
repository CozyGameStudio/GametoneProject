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
    public Transform foodHolder;
    [Header("Character")]
    public float speed = 5f;
    public NavMeshAgent agent;

    private GameObject customerTablePlace;
    private GameObject customerBackPlace;
    private int customerChairPlaceLength;
    private Food orderFood;
    private bool isOrdered = false;
    public int tableNumber{get;private set;}
    private bool receiveOrder=false;
    private float initSpeed;
    private Animator animator;
    private void Awake()
    {
        fsm = new StateMachine<States, StateDriverUnity>(this);
        fsm.ChangeState(States.Idle);
    }
    void Start(){
        orderFood = DataManager.Instance.RandomFood();
        Debug.Log(orderFood.foodData.foodName);
        animator=GetComponent<Animator>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
    }
    private void Update()
    {
        fsm.Driver.Update.Invoke();
        SetAnimation(agent.velocity);
    }
    public void MultSpeed(float mult)
    {
        speed *= mult;
        agent.speed = speed;
    }
    public void BackToNormalSpeed()
    {
        speed = initSpeed;
        agent.speed = speed;
    }
    public void SetAnimation(Vector3 currentVelocity)
    {
        if (animator == null) { Debug.Log("animator is null"); return; }
        if (currentVelocity.y > 0.1)
        {//towards upside
            animator.SetFloat("YVelocity", 1);
        }
        else if (currentVelocity.y < -0.1)
        {//towards downward
            animator.SetFloat("YVelocity", -1);
        }
        else
            animator.SetFloat("YVelocity", 0);
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
                    if (!CustomerManager.Instance.customerChairPresent[i])
                    {
                        tableNumber = i+1;
                        customerTablePlace = CustomerManager.Instance.customerChair[i];
                        CustomerManager.Instance.customerChairPresent[i] = true;
                        break;
                    }
                }
                fsm.ChangeState(States.Walk);

            }
        }
        // If an order is placed, add money and receive the return location for a move call 
        else
        {
            BusinessGameManager.Instance.AddMoney(orderFood.currentValue);
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
                CustomerManager.Instance.customerChairPresent[tableNumber-1] = false;
                
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
        transform.parent.SetParent(customerTablePlace.transform);
        
        isOrdered = true;
        OrderBoard newOrder=new OrderBoard(orderFood,tableNumber);
        OrderManager.Instance.PutOrderInQueue(newOrder);
        transform.GetComponentInParent<CustomerTable>().SetBubble(orderFood,2);
    }
    void Order_Update()
    {
        if(receiveOrder)
        {
            fsm.ChangeState(States.Idle);
        }
    }

    void Order_Exit()
    {
        transform.parent.SetParent(null);
    }
    public void GetMenu(GameObject menu)
    {
        menu.transform.SetParent(foodHolder);
        menu.transform.position=foodHolder.position;
        receiveOrder = true;
        
    }

}
