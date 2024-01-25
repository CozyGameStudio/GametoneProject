using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;
using UnityEngine.AI;
public class Chef : MonoBehaviour
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

    private List<GameObject> serveTables;
    private List<GameObject> foodPlaces;
    private List<GameObject> servePlaces;

    [Header("Character")]
    public Character character;
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
    private SpriteRenderer spriteRenderer;
    private float initSpeed;
    void Awake()
    {
        fsm=new StateMachine<States, StateDriverUnity>(this);
    }
    private void Start() {
        initSpeed=speed;
        fsm.ChangeState(States.Idle);
        agent = GetComponent<NavMeshAgent>();
        spriteRenderer=GetComponent<SpriteRenderer>();
        serveTables=ServerManager.Instance.serveTables;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        agent.speed=speed;
        foreach (var serveTable in serveTables){
            AddChildrenWithName(serveTable, "FoodHolder");
        }
    }
    public void AddChildrenWithName(GameObject parent, string nameToFind)
    {
        if (foodPlaces == null)
            foodPlaces = new List<GameObject>();
        if (servePlaces == null)
            servePlaces = new List<GameObject>();
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            Transform child = parent.transform.GetChild(i);
            if (child.name.Contains(nameToFind))
            {
                foodPlaces.Add(child.GetChild(0).gameObject);
                servePlaces.Add(child.GetChild(1).gameObject);
            }
        }
    }
    public void MultSpeed(float mult){
        speed*=mult;
        agent.speed=speed;
    }
    public void BackToNormalSpeed(){
        speed=initSpeed;
        agent.speed = speed;
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
        Vector3 currentVelocity = agent.velocity;
        if (currentVelocity.magnitude > 0.1f)
        {
            transform.localScale= currentVelocity.x < 0?new Vector3(-1,1,1): new Vector3(1, 1, 1);
        }
    
    }
    void Idle_Enter()
    {
        //playanimation(idle)
        
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
        
        agent.SetDestination(placeToMove.position);
    }
    void Walk_Update()
    {
        //Until chef arrives cook Place
        if(Vector2.Distance(transform.position,placeToMove.position)<.5f)
        {
            if (!isHolding)
            {
                fsm.ChangeState(States.Cook);
            }
            else
            {
                fsm.ChangeState(States.Serve);
            }
        }
    }
    void Cook_Enter(){
        Food foodToMake = currentMenu.foodData;
        StartCoroutine(cookCoroutine(foodToMake, nowUsing.currentCookTime));
    }
    IEnumerator cookCoroutine(Food foodToMake, float cooktime){
        isCooking=true;
        yield return new WaitForSeconds(cooktime);
        GameObject foodMade = Instantiate(Resources.Load<GameObject>(foodToMake.foodData.foodName), foodHolder.transform.position, Quaternion.identity);
        foodMade.GetComponent<FoodToServe>().orderstatus=currentMenu;
        foodMade.transform.parent = foodHolder.transform;
        isCooking=false;
        currentMenu=default;
    }
    void Cook_Update(){
        if(!isCooking){
            for (int i = 0; i < foodPlaces.Count; i++)
            {
                if (foodPlaces[i].GetComponent<FoodPlace>().IsAvailable)
                {
                    serveHolder = foodPlaces[i].gameObject;
                    placeToMove = servePlaces[i].transform;
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
        
    }
    void Serve_Enter()
    {
        
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
        
    }
    public void ReceiveOrder(OrderBoard order,Machine appropriateMachine)
    {
        currentMenu = order;
        nowUsing= appropriateMachine;
        nowUsing.SwitchTakenPlace();
        placeToMove=nowUsing.transform;
        IsAvailable=false;
        fsm.ChangeState(States.Walk);
    }

    public void SetAvailable(){
        IsAvailable=true;
        OrderManager.Instance.ChefAvailable();
    }
}
