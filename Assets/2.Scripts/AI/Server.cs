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

    }

    void Update()
    {
        fsm.Driver.Update.Invoke();
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

    void Idle_Enter()
    {
        
        //playanimation(idle)w
        
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
        SetAvailable();
        
    }
    // Change server status to available
    public void SetAvailable()
    {
        IsAvailable = true;
        isThereMenuToServe=false;
        OnAvailable?.Invoke();
    }
}
