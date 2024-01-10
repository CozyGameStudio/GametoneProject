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

    [Header("Character")]
    public float speed = 5f;

    private GameObject customerTablePlace;
    private GameObject customerBackPlace;
    private int customerTablePlaceLength;
    private FoodTest orderFood;
    private bool isOrdered = false;
    private int tableNumber;

    private void Awake()
    {
        orderFood = DataManager.Instance.Food;
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
        /* �ֹ��� ���� �ʾ��� ��� ���̺� ��ġ�� �޾� �̵� ȣ��*/
        if (!isOrdered)
        {
            Debug.Log(CustomerManager.Instance.IsCustomerFull());
            if (!CustomerManager.Instance.IsCustomerFull())
            {
                customerTablePlaceLength = CustomerManager.Instance.customerTablePlace.Length;
                for (int i = 0; i < customerTablePlaceLength; i++)
                {
                    if (!CustomerManager.Instance.customerTablePresent[i])
                    {
                        tableNumber = i;
                        customerTablePlace = CustomerManager.Instance.customerTablePlace[i];
                        CustomerManager.Instance.customerTablePresent[i] = true;
                        break;
                    }
                }
                fsm.ChangeState(States.Walk);

            }
        }
        /* �ֹ��� ���� ��� ���� �߰��ϰ� �ǵ��ư��� ��ġ�� �޾� �̵� ȣ��*/
        else
        {
            Debug.Log("+" + orderFood.money);
            customerBackPlace = CustomerManager.Instance.customerBackPlace;
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
        /* �ֹ��� ������ ��� ���̺�� �̵�*/
        if (!isOrdered)
        {
            if (Vector2.Distance(transform.position, customerTablePlace.transform.position) > 0.1f)
            {
                transform.position = Vector2.MoveTowards(transform.position, customerTablePlace.transform.position, speed * Time.deltaTime);
            }
            else
            {
                fsm.ChangeState(States.Order);
            }
        }
        /* �ֹ��� ���� ��� �ǵ��ư��� ��ġ�� �̵�*/
        else
        {
            if (Vector2.Distance(transform.position, customerBackPlace.transform.position) > 0.1f)
            {
                transform.position = Vector2.MoveTowards(transform.position, customerBackPlace.transform.position, speed * Time.deltaTime);
            }
            else
            {
                CustomerManager.Instance.customerTablePresent[tableNumber] = false;
                Debug.Log("Destory");
                Destroy(gameObject);
            }
        }
        
    }
    void Walk_Exit()
    {
        Debug.Log("walk exit");
    }

    IEnumerator Order_Enter()
    {
        Debug.Log("Order Enter");
        isOrdered = true;
        yield return new WaitForSeconds(orderFood.time);
        fsm.ChangeState(States.Idle);
    }

    void Order_Exit()
    {
        Debug.Log("Order exit");
    }
}
