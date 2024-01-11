using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;
using UnityEngine.AI;
using UnityEditor.Rendering;
using Unity.VisualScripting;

public struct OrderBoard{
    public string name{get;private set;}
    public int tableNumber { get; private set;}
    public OrderBoard(string nam,int num){
        name=nam;
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
    private int customerTablePlaceLength;
    private string orderFood;
    private bool isOrdered = false;
    public int tableNumber{get;private set;}
    private bool receiveOrder=false;
    FoodMain receivedFood;

    private void Awake()
    {
        orderFood = DataManager.Instance.RandomFood();//receive name from data manaager
        receivedFood= DataManager.Instance.FindFoodWithName(orderFood);
        fsm = new StateMachine<States, StateDriverUnity>(this);
        fsm.ChangeState(States.Idle);
        orderBubble.SetActive(false);
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
                        tableNumber = i+1;
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
            GameManager.Instance.AddMoney(receivedFood.FoodData.Money);
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
        /* �ֹ��� ������ ��� ���̺��� �̵�*/
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
                CustomerManager.Instance.customerTablePresent[tableNumber-1] = false;
                Debug.Log("Destory");
                Destroy(gameObject);
            }
        }
    }
    void Walk_Exit()
    {
        Debug.Log("walk exit");
    }

    void Order_Enter()
    {
        transform.SetParent(customerTablePlace.transform);
        Debug.Log("Order Enter");
        isOrdered = true;
        OrderBoard newOrder=new OrderBoard(orderFood,tableNumber);
        OrderManager.Instance.putOrderInQueue(newOrder);
        orderBubble.SetActive(true);
        foodRenderer.sprite=receivedFood.FoodData.Icon;
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
        Debug.Log("Order exit");
    }
    public void GetMenu(GameObject menu)
    {
        if (menu == null)
        {
            Debug.LogError("Received menu is null");
        }
        if (foodHolder == null)
        {
            Debug.LogError("foodHolder is null");
        }

        menu.transform.SetParent(foodHolder);
        receiveOrder = true;
        Debug.Log("Menu received");
    }

}
