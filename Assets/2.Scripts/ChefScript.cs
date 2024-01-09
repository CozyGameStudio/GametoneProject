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

    [Header("PlaceHolder")]
    public Machine[] machines;
    public GameObject[] foodPlace;
    public GameObject[] servePlace;

    [Header("Character")]
    public float speed=5f;
    public GameObject foodHolder;

    private bool isTakenPlace=false;
    private bool isHolding=false;
    private Transform placeToMove; //Chef cook place
    private GameObject serveHolder;
    private string currentMenu;
    void Awake()
    {
        fsm=new StateMachine<States, StateDriverUnity>(this);
        fsm.ChangeState(States.Idle);
    }
    void Update()
    {
        fsm.Driver.Update.Invoke();
    }
    void Idle_Enter()
    {
        //playanimation(idle)
        Debug.Log("Idle Enter");
    }
    void Idle_Update()
    {
        Debug.Log(OrderManager.Instance.isQueueEmpty());
        if(!OrderManager.Instance.isQueueEmpty()){ 
            for(int i=0;i<machines.Length;i++){
                if(!machines[i].isTakenPlace){
                    placeToMove=machines[i].gameObject.transform;
                    Debug.Log(machines[i].gameObject.ToString());
                    currentMenu=OrderManager.Instance.takeOrderInQueue();
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
    IEnumerator Cook_Enter(){
        string foodName = OrderManager.Instance.takeOrderInQueue();
        Debug.Log("Cook Enter");
        Debug.Log($"He is cooking {foodName}");
        yield return new WaitForSeconds(3);
        GameObject food = Instantiate(Resources.Load<GameObject>(foodName),foodHolder.transform.position,Quaternion.identity);
        //potatosoup will be changed to foodName
        food.transform.parent = foodHolder.transform;
        for (int i = 0; i < foodPlace.Length; i++)
        {
            if (foodPlace[i].transform.childCount==0)
            {
                serveHolder = foodPlace[i];
                placeToMove = servePlace[i].transform;
                break;
            }
        }
        fsm.ChangeState(States.Walk);
    }
    void Serve_Enter()
    {
        Debug.Log("Serve Enter");
        foodHolder.transform.GetChild(0).parent = serveHolder.transform;
        fsm.ChangeState(States.Idle);
        //if (!OrderManager.Instance.isQueueEmpty())
        //{
        //    for (int i = 0; i < machines.Length; i++)
        //    {
        //        if (!machines[i].isTakenPlace)
        //        {
        //            placeToMove = machines[i].transform;
        //            currentMenu = OrderManager.Instance.takeOrderInQueue();
        //            break;
        //        }
        //    }
        //    fsm.ChangeState(States.Walk);
        //}
        //else
        //{
        //    fsm.ChangeState(States.Idle);
        //}
    }
    void Serve_Exit()
    {
        Debug.Log("Serve Exit");
    }
   
    

}
