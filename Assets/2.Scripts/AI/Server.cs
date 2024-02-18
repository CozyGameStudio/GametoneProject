using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;
using System;
using UnityEngine.AI;
public class Server : MonoBehaviour
{
    public enum States //state enum
    {
        Idle,
        Walk,
        Find,
        Serve
    }
    [Header("캐릭터")]
    public Character character;
    StateMachine<States, StateDriverUnity> fsm;
    public GameObject foodHolder;
    public float speed=2f;
    public bool IsAvailable { get; private set; } = true; // Check the server's availability
    public NavMeshAgent agent;
    private bool isThereMenuToServe=false;
    private GameObject menuToServe;
    private Transform placeToMove; //Server bring food place
    private Customer currentCustomer;
    public event Action OnAvailable;
    private float initSpeed;
    private Animator animator;
    private Transform initPosition;

    public bool isPickupForTutorial = false;
    public bool isServedForTutorial = false;

    void Awake()
    {
        initPosition = new GameObject("InitPosition").transform;
        initPosition.position = agent.transform.position;
        initPosition.rotation = agent.transform.rotation;
        fsm = new StateMachine<States, StateDriverUnity>(this);
        fsm.ChangeState(States.Idle);
    }
    void Start(){
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        initSpeed=speed;
        Debug.Log(initPosition.position+" "+initPosition.rotation);
        animator=GetComponent<Animator>();
    }

    void Update()
    {
        fsm.Driver.Update.Invoke();
        Vector3 currentVelocity = agent.velocity;
        SetAnimation(currentVelocity);
    }
    void OnEnable()
    {
        DataManager.Instance.OnRewardActivatedDelegate += FeverTime;
        SetAvailable();
    }
    void OnDisable()
    {
        if (DataManager.Instance != null) DataManager.Instance.OnRewardActivatedDelegate -= FeverTime;
        IsAvailable = false;
    }
    private void FeverTime(bool isActivated)
    {
        speed = isActivated ? speed *= 2 : speed *= .5f;
        if (animator != null) animator.speed = isActivated ? 2 : 1;
        agent.speed = speed;
    }
    public void HandleNewServeTask(Transform child)
    {
        isThereMenuToServe=true;
        menuToServe = child.gameObject;
        IsAvailable=false;
        
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
        if(animator==null)return;
        if (currentVelocity.magnitude.Equals(0))
        {//towards upside
            animator.SetFloat("YVelocity", 0);
        }
        else if (currentVelocity.y >0)
        {//towards downward
            animator.SetFloat("YVelocity", 1);
        }
        else
            animator.SetFloat("YVelocity", -1);
    }
    void Idle_Enter(){
        placeToMove = initPosition;
        agent.SetDestination(placeToMove.position);
    }
    void Idle_Update()
    {
        if (isThereMenuToServe)
        {
            placeToMove=menuToServe.transform;
            fsm.ChangeState(States.Walk);
        }
    }
    void Walk_Enter()
    {
        Debug.Log("WalkEnter");
        agent.SetDestination(placeToMove.position);
    }
    void Walk_Update()
    {
        isPickupForTutorial = true;
        if (Vector2.Distance(transform.position, placeToMove.position) < 1.5f)
        {
            Debug.Log("State Find EnterTry");
            fsm.ChangeState(States.Find);
        }
    }
    IEnumerator Find_Enter(){
        Debug.Log("State Find Enter");
        FoodPlace foodPlace = menuToServe.GetComponentInParent<FoodPlace>();
        if (foodPlace != null)
        {
            foodPlace.RemoveChild(menuToServe);
        }
        menuToServe.transform.parent = foodHolder.transform;
        menuToServe.transform.position = foodHolder.transform.position;
        int tableNum = menuToServe.GetComponent<FoodToServe>().orderstatus.tableNumber;
        Debug.Log("Trying Find Customer");
        FindCustomer(tableNum);
        float retryInterval = 1f;
        while (currentCustomer == null)
        {
            Debug.Log("Trying Find Customer");
            yield return new WaitForSeconds(retryInterval);
            FindCustomer(tableNum);
        }
        agent.SetDestination(placeToMove.position);
        fsm.ChangeState(States.Serve);
    }
    
    public void FindCustomer(int tableNum){
        foreach (var chair in CustomerManager.Instance.customerChair)
        {
            if (chair.transform.childCount > 0 && chair.transform.GetComponentInChildren<Customer>().tableNumber == tableNum)
            {
                placeToMove = chair.transform.GetComponentInParent<CustomerTable>().servePosition;//guest place
                currentCustomer = chair.transform.GetComponentInChildren<Customer>();
                break;
            }
        }
    }

    void Serve_Update()
    {
        if (agent.velocity.y > 0.01)
        {//towards upside
            if (menuToServe != null) menuToServe.SetActive(false);
        }
        else
        {//towards downward
            if (menuToServe != null) menuToServe.SetActive(true);
        }

        if (Vector2.Distance(transform.position, placeToMove.position) < .1f)
        {
            if (currentCustomer == null)
            {
                return; // Stop execution of subsequent code
            }
            fsm.ChangeState(States.Idle);
        }
    }
    void Serve_Exit()
    {
        menuToServe.SetActive(true);
        currentCustomer.GetMenu(menuToServe);
        currentCustomer=null;
        menuToServe=null;
        SetAvailable();
        isServedForTutorial = true;
    }
    public void SetAvailable()
    {
        IsAvailable = true;
        isThereMenuToServe=false;
        OnAvailable?.Invoke();
    }
    public void SetSpeed(float speedMultiplier)
    {
        speed = initSpeed * speedMultiplier;
        agent.speed = speed;
    }
}
