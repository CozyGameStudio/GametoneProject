using UnityEngine;
using MonsterLove.StateMachine;
using UnityEngine.AI;
using DG.Tweening;

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
    public float speed = 1.5f;
    public NavMeshAgent agent;
    public SpriteRenderer likeParticle;

    private GameObject customerTablePlace;
    private GameObject customerBackPlace;
    private int customerChairPlaceLength;
    private Food orderFood;
    private bool isOrdered = false;
    public int tableNumber{get;private set;}
    private bool receiveOrder=false;
    private float initSpeed=1.5f;
    private Animator animator;
    private GameObject foodToHold;
    void OnEnable()
    {
        if (DataManager.Instance != null) DataManager.Instance.OnRewardActivatedDelegate += FeverTime;
    }
    void OnDisable()
    {
        if (DataManager.Instance != null) DataManager.Instance.OnRewardActivatedDelegate -= FeverTime;
    }
    private void FeverTime(bool isActivated)
    {
        speed = isActivated ? initSpeed * 2 : initSpeed;
        if(animator!=null)animator.speed=isActivated? 2:1;
        agent.speed = speed;
    }
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
        likeParticle.color=new Color(likeParticle.color.r, likeParticle.color.g, likeParticle.color.b,0);
        if (DataManager.Instance != null) FeverTime(DataManager.Instance.isSpeedRewardActivated);
    }
    private void Update()
    {
        fsm.Driver.Update.Invoke();
        SetAnimation(agent.velocity);
    }
    public void SetAnimation(Vector3 currentVelocity)
    {
        if (animator == null) { Debug.Log("animator is null"); return; }
        if (currentVelocity.y > 0.1)
        {//towards upside
            animator.SetFloat("YVelocity", 1);
            if(foodToHold!=null)foodToHold.SetActive(false);
        }
        else if (currentVelocity.y < -0.1)
        {//towards downward
            animator.SetFloat("YVelocity", -1);
            if (foodToHold != null) foodToHold.SetActive(true);
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
            int payMoney=CustomerManager.Instance.isRewardActivated? orderFood.currentValue*2 : orderFood.currentValue;
            BusinessGameManager.Instance.AddMoney(payMoney);
            StageMissionManager.Instance.IncreaseAccumulatedCustomer();
            StageMissionManager.Instance.IncreaseAccumulatedSales(payMoney);
            SystemManager.Instance.PlaySFXByName("pay");
            Debug.Log(payMoney);
            customerBackPlace = CustomerManager.Instance.customerBackPlace;
            fsm.ChangeState(States.Walk);
        }
    }

    void Walk_Enter(){
        if (isOrdered){
            CustomerManager.Instance.customerChairPresent[tableNumber - 1] = false;
            Sequence seq = DOTween.Sequence()
       .Append(likeParticle.DOFade(1, .2f))
       .Append(likeParticle.transform.DOScale(.8f,3f)).SetEase(Ease.InQuad)
       .Append(likeParticle.DOFade(0, .3f));
            seq.Play();
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
                Destroy(transform.parent.gameObject);
            }
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
        foodToHold=menu;
        foodToHold.transform.SetParent(foodHolder);
        foodToHold.transform.position=foodHolder.position;
        receiveOrder = true;
       
    }
}
