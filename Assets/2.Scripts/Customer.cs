using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;
using UnityEngine.AI;
using UnityEditor.Rendering;
using Unity.VisualScripting;

public class Customer : MonoBehaviour
{ 
    public enum States { 
        Idle,
        Walk,
        Order
    }

    StateMachine<States, StateDriverUnity> fsm;

    [Header("PlaceHolder")]

    [Header("Character")]
    public float speed = 5f;

    private GameObject customerPlace;
    private int customerPlaceLength;

    private void Awake()
    {
        fsm = new StateMachine<States, StateDriverUnity>(this);
        fsm.ChangeState(States.Idle);
    }
    private void Update()
    {
        fsm.Driver.Update.Invoke();
    }
    void Idle_Enter()
    {
        Debug.Log("Idle Enter");
    }
    void Idle_Update() 
    {
        Debug.Log(CustomerManager.Instance.IsCustomerFull());
        if (!CustomerManager.Instance.IsCustomerFull())
        {
            customerPlaceLength = CustomerManager.Instance.customerPlace.Length;
            for(int i = 0; i < customerPlaceLength; i++)
            {
                if (!CustomerManager.Instance.customerPresent[i])
                {
                    customerPlace = CustomerManager.Instance.customerPlace[i];
                }
                CustomerManager.Instance.customerPresent[i] = true;
            }
            fsm.ChangeState(States.Walk);

        }
    }
    void Idle_Exit()
    {
        Debug.Log("Idle Exit");
    }
    void Walk_Enter()
    {
        Debug.Log("Walk Enter");
    }
    void Walk_Update()
    {
        //Until chef arrives cook Place
        if (Vector2.Distance(transform.position, customerPlace.transform.position) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, customerPlace.transform.position, speed * Time.deltaTime);

        }
        else
        {
            fsm.ChangeState(States.Order);
        }
    }
    void Walk_Exit()
    {
        Debug.Log("walk exit");
    }

    void Order_Enter()
    {
        Debug.Log("Order Enter");
    }
    void Order_Update()
    {

    }
    void Order_Exit()
    {
        Debug.Log("Order exit");
    }
}
