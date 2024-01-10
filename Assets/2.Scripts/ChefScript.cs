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
    public Machine[] machines;
    public GameObject[] foodPlace;
    public GameObject[] servePlace;

    [Header("Character")]
    public float speed=5f;
    public GameObject foodHolder;

    public bool IsAvailable => fsm.State.Equals(States.Idle);
    private bool isTakenPlace=false;
    private bool isHolding=false;
    private Transform placeToMove; //Chef cook place
    private GameObject serveHolder;
    private string currentMenu="";
    private bool isCooking=false;
    void Awake()
    {
        fsm=new StateMachine<States, StateDriverUnity>(this);
        
        
    }
    private void Start() {
        if (OrderManager.Instance != null)
        {
            OrderManager.Instance.RegisterChef(this);
            OrderManager.Instance.OnNewOrder += HandleNewOrder;
        }
        fsm.ChangeState(States.Idle);
    }
    void Update()
    {
        fsm.Driver.Update.Invoke();
    }
    public void HandleNewOrder(string order)
    {
        currentMenu=order;
    }
    void Idle_Enter()
    {
        //playanimation(idle)
        Debug.Log("Idle Enter");
        OnChefAvailable?.Invoke();
    }
    void Idle_Update()
    {
        if (this.IsAvailable&&!currentMenu.Equals(""))
        { 
            for(int i=0;i<machines.Length;i++){
                if(!machines[i].isTakenPlace){
                    placeToMove=machines[i].gameObject.transform;
                    Debug.Log(machines[i].gameObject.ToString());
                    break;
                }
            }
            fsm.ChangeState(States.Walk);
        }
    }
    void Idle_Exit()
    {
        Debug.Log("Idle Exit");
    }
    public void NewOrderAvailable(string order)
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
        Debug.Log("Walk Enter");
    }
    void Walk_Update()
    {
        //Until chef arrives cook Place
        if(Vector2.Distance(transform.position,placeToMove.position)>0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, placeToMove.position, speed * Time.deltaTime);
        
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
        Debug.Log("walk exit");
    }
    void Cook_Enter(){
        string foodName = currentMenu;
        Debug.Log("Cook Enter");
        Debug.Log($"He is cooking {foodName}");
        StartCoroutine(cookCoroutine(foodName,3));
    }
    IEnumerator cookCoroutine(string foodName,int cooktime){
        isCooking=true;
        yield return new WaitForSeconds(cooktime);
        GameObject food = Instantiate(Resources.Load<GameObject>(foodName), foodHolder.transform.position, Quaternion.identity);
        food.transform.parent = foodHolder.transform;
        isCooking=false;
        currentMenu="";
    }
    void Cook_Update(){
        if(!isCooking){
            for (int i = 0; i < foodPlace.Length; i++)
            {
                if (foodPlace[i].transform.childCount == 0)
                {
                    serveHolder = foodPlace[i];
                    placeToMove = servePlace[i].transform;
                    break;
                }
            }
            fsm.ChangeState(States.Walk);
        }
    }
    void Cook_Exit(){
        isHolding=true;
        Debug.Log("cook exit");
    }
    void Serve_Enter()
    {
        Debug.Log("Serve Enter");
        GameObject comFood= foodHolder.transform.GetChild(0).gameObject;
        comFood.transform.parent = serveHolder.transform;
        comFood.transform.position= serveHolder.transform.position;
        fsm.ChangeState(States.Idle);
    }
    void Serve_Exit()
    {
        isHolding=false;
        Debug.Log("Serve Exit");
    }
}
