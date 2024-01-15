using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;
using UnityEngine.AI;
public class ChefScript : MonoBehaviour
{
    public enum States //state enum
    {
        Idle,
        Walk,
        Cook,
        Serve
    }

    StateMachine<States, StateDriverUnity> fsm;

    // Define a delegate
    public delegate void AvailableHandler();

    // Create an event based on the delegate
    public event AvailableHandler OnChefAvailable;

    [Header("PlaceHolder")]
    public GameObject[] foodPlace;
    public GameObject[] servePlace;

    [Header("Character")]
    public float speed=5f;
    public GameObject foodHolder;

    public bool IsAvailable { get; private set; } = true;
    private Machine nowUsing;
    private bool isHolding=false;
    private Transform placeToMove; //Chef cook place
    private GameObject serveHolder;
    private OrderBoard currentMenu =default;
    private bool isCooking=false;
    private NavMeshAgent agent;

    void Awake()
    {
        fsm=new StateMachine<States, StateDriverUnity>(this);
    }
    private void Start() {
        fsm.ChangeState(States.Idle);
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }
    void OnEnable(){
        SetAvailable();

    }
    void OnDisable(){
        IsAvailable = false;
    }
    void Update()
    {
        fsm.Driver.Update.Invoke();
    }
    void Idle_Enter()
    {
        //playanimation(idle)
        //Debug.Log("Idle Enter");
    }
    public void NewOrderAvailable(OrderBoard order)
    {
        if (this.IsAvailable)
        {
            fsm.ChangeState(States.Idle);
        }
        currentMenu=order;
    }

    void Walk_Enter()
    {
        //playanimation(walk)
        //Debug.Log("Walk Enter");
    }
    void Walk_Update()
    {
        //Until chef arrives cook Place
        if(Vector2.Distance(transform.position,placeToMove.position)>1f)
        {
            agent.SetDestination(placeToMove.position);
        }
        else
        {
            if(!isHolding){
                fsm.ChangeState(States.Cook);
            }
            else{
                fsm.ChangeState(States.Serve);
            }
            
        }
    }
    void Walk_Exit(){
        //Debug.Log("walk exit");
    }
    void Cook_Enter(){
        string foodName = currentMenu.name;
        //Debug.Log("Cook Enter");
        //Debug.Log($"He is cooking {foodName}");
        StartCoroutine(cookCoroutine(foodName,3));
    }
    IEnumerator cookCoroutine(string foodName,int cooktime){
        isCooking=true;
        yield return new WaitForSeconds(cooktime);
        GameObject food = Instantiate(Resources.Load<GameObject>(foodName), foodHolder.transform.position, Quaternion.identity);
        food.GetComponent<FoodMain>().orderstatus=currentMenu;
        food.transform.parent = foodHolder.transform;
        isCooking=false;
        currentMenu=default;
    }
    void Cook_Update(){
        if(!isCooking){
            for (int i = 0; i < foodPlace.Length; i++)
            {
                if (foodPlace[i].GetComponent<FoodPlace>().IsAvailable)
                {
                    serveHolder = foodPlace[i].gameObject;
                    placeToMove = servePlace[i].transform;
                    break;
                }
            }
            fsm.ChangeState(States.Walk);
        }
    }
    void Cook_Exit(){
        isHolding=true;
        nowUsing.SwitchTakenPlace();
        serveHolder.GetComponent<FoodPlace>().IsAvailable=false;
        //Debug.Log("cook exit");
    }
    void Serve_Enter()
    {
        //Debug.Log("Serve Enter");
        GameObject comFood= foodHolder.transform.GetChild(0).gameObject;
        // comFood.transform.parent = serveHolder.transform;
        comFood.transform.position= serveHolder.transform.position;
        serveHolder.GetComponent<FoodPlace>().AddChild(comFood);
        fsm.ChangeState(States.Idle);
    }
    void Serve_Exit()
    {
        isHolding=false;
        SetAvailable();
        //Debug.Log("Serve Exit");
    }
    // 서버 상태를 사용 가능으로 변경하는 메소드
    public void ReceiveOrder(OrderBoard order,Machine appropriateMachine)
    {
        Debug.Log("ReceiveOrder called. Order: " + order + ", Machine: " + appropriateMachine);
        currentMenu = order;
        nowUsing= appropriateMachine;
        nowUsing.SwitchTakenPlace();
        placeToMove=nowUsing.transform;
        IsAvailable=false;
        fsm.ChangeState(States.Walk); // 예시로, 상태를 변경하여 셰프가 오더를 처리하도록 합니다.
    }

    public void SetAvailable(){
        IsAvailable=true;
        OrderManager.Instance.ChefAvailable();
    }
}
