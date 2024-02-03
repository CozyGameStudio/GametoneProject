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
        Serve
    }

    StateMachine<States, StateDriverUnity> fsm;
    public GameObject foodHolder;
    public float speed=2f;
    public bool IsAvailable { get; private set; } = true; // Check the server's availability
    private bool isThereMenuToServe=false;
    private GameObject menuToServe;
    private Transform placeToMove; //Server bring food place
    private Customer currentCustomer;
    private NavMeshAgent agent;
    public event Action OnAvailable;
    private float initSpeed;
    private Animator animator;
    void Awake()
    {
        fsm = new StateMachine<States, StateDriverUnity>(this);
        fsm.ChangeState(States.Idle);
    }
    void Start(){
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        initSpeed=speed;
        animator=GetComponent<Animator>();
    }

    void Update()
    {
        fsm.Driver.Update.Invoke();
        Vector3 currentVelocity = agent.velocity;
        SetAnimation();
    }
    private void OnEnable() {
        SetAvailable();    
    }
    private void OnDisable()
    {
        IsAvailable = false;
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
    public void SetAnimation(){
        if(animator==null)return;
        if (agent.velocity.y > 0.1)
        {//towards upside
            animator.SetFloat("YVelocity", 1);
            if (menuToServe != null) menuToServe.SetActive(false);
        }
        else if (agent.velocity.y < -0.1)
        {//towards downward
            animator.SetFloat("YVelocity", -1);
            if (menuToServe != null) menuToServe.SetActive(false);
        }
        else
            animator.SetFloat("YVelocity", 0);
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
        agent.SetDestination(placeToMove.position);
    }
    void Walk_Update()
    {
        if (Vector2.Distance(transform.position, placeToMove.position) < 1.5f)
        {
            fsm.ChangeState(States.Serve);
        }
    }
    void Serve_Enter()
    {
        FoodPlace foodPlace = menuToServe.GetComponentInParent<FoodPlace>();
        if (foodPlace != null)
        {
            foodPlace.RemoveChild(menuToServe);
        }
        menuToServe.transform.parent=foodHolder.transform;
        menuToServe.transform.position=foodHolder.transform.position;
        int tableNum=menuToServe.GetComponent<FoodToServe>().orderstatus.tableNumber;
        foreach(var chair in CustomerManager.Instance.customerChair)
        {
            if(chair.transform.childCount>0&&chair.transform.GetChild(0).GetComponent<Customer>().tableNumber==tableNum){
                placeToMove=chair.transform.parent;//guest place
                currentCustomer = chair.transform.GetChild(0).GetComponent<Customer>();
                break;
            }
        }
        agent.SetDestination(placeToMove.position);
        //playanimation(Serve)
        
    }
    void Serve_Update()
    {
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
        currentCustomer.GetMenu(menuToServe);
        menuToServe=null;
        SetAvailable();
        
    }
    // Change server status to available
    // Change server status to available
    public void SetAvailable()
    {
        IsAvailable = true;
        isThereMenuToServe=false;
        OnAvailable?.Invoke();
    }
}
