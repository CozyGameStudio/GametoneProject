using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;
using UnityEngine.AI;
using System;
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
    

    [Header("캐릭터")]
    public Character character;
    public float initSpeed=1.5f;
    private float speed=1.5f;
    public GameObject foodHolder;
    public NavMeshAgent agent;
    public Animator loadingBarAnimator;
    public bool IsAvailable { get; private set; } = true;
    private IMachineInterface nowUsingMachine;
    private bool isHolding=false;
    private Transform placeToMove; //Chef cook place
    private GameObject serveHolder;
    private OrderBoard currentMenu =default;
    private bool isCooking=false;
    private Animator animator;
    private Transform initPosition;
    private GameObject foodObject;
    void OnEnable()
    {
        if (DataManager.Instance != null) DataManager.Instance.OnRewardActivatedDelegate += FeverTime;
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
        if(animator!=null)animator.speed=isActivated?2:1;
        agent.speed=speed;
    }
    void Awake()
    {
        initPosition = new GameObject("InitPosition").transform;
        initPosition.position = agent.transform.position;
        initPosition.rotation = agent.transform.rotation;
        fsm =new StateMachine<States, StateDriverUnity>(this);
    }
    private void Start() {
        fsm.ChangeState(States.Idle);
        serveTables=ServerManager.Instance.serveTables;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        foreach (var serveTable in serveTables){
            AddChildrenWithName(serveTable, "FoodHolder");
        }
        animator=GetComponent<Animator>();
    }
    public void AddChildrenWithName(GameObject parent, string nameToFind)
    {
        if (foodPlaces == null)
            foodPlaces = new List<GameObject>();
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            Transform child = parent.transform.GetChild(i);
            if (child.name.Contains(nameToFind))
            {
                foodPlaces.Add(child.GetChild(0).gameObject);
            }
        }
    }
    public void SetAnimation(Vector3 currentVelocity)
    {
        if (animator == null) {Debug.Log("animator is null");return;}
        if (currentVelocity.magnitude.Equals(0))
        {
            animator.SetFloat("YVelocity", 0);
        }
        else if (currentVelocity.y > 0)
        {
            if(foodObject!=null)foodObject.SetActive(false);
            animator.SetFloat("YVelocity", 1);
        }
        else{
            if (foodObject != null) foodObject.SetActive(true);
            animator.SetFloat("YVelocity", -1);
        }
    }

    void Update()
    {
        fsm.Driver.Update.Invoke();
        Vector3 currentVelocity = agent.velocity;
        SetAnimation(currentVelocity);
    
    }
    void Idle_Enter()
    {
        placeToMove = initPosition;
        agent.SetDestination(placeToMove.position);
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
        agent.SetDestination(placeToMove.position);
    }
    void Walk_Update()
    {
        if(!isHolding)
        {
            if (Vector2.Distance(transform.position, placeToMove.position) < .2f){
                fsm.ChangeState(States.Cook);
            }
        } 
        else{
            if (Vector2.Distance(transform.position, placeToMove.position) < 1.5f){
                fsm.ChangeState(States.Serve);
            }
        }
    }
    void Cook_Enter(){
        Food foodToMake = currentMenu.foodData;
        float cookTime= OrderManager.Instance.isRewardActivated? nowUsingMachine.currentCookTime * 0.5f : nowUsingMachine.currentCookTime;
        StartCoroutine(cookCoroutine(foodToMake, cookTime));
    }
    IEnumerator cookCoroutine(Food foodToMake, float cookTime){
        isCooking=true;
        loadingBarAnimator.gameObject.SetActive(true);
        float animationSpeed = 60 / cookTime; 
        loadingBarAnimator.speed = animationSpeed;
        float elapsedTime = 0;
        while (elapsedTime < cookTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        loadingBarAnimator.gameObject.SetActive(false);
        
        foodObject = Instantiate(Resources.Load<GameObject>("FoodToServe"), foodHolder.transform.position, Quaternion.identity);
        foodObject.GetComponent<SpriteRenderer>().sprite= foodToMake.foodData.foodIcon;
        foodObject.GetComponent<FoodToServe>().orderstatus=currentMenu;
        foodObject.transform.parent = foodHolder.transform;
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
                    placeToMove = serveHolder.transform;
                    break;
                }
            }
            fsm.ChangeState(States.Walk);
        }
    }
    void Cook_Exit(){
        isHolding=true;
        nowUsingMachine.SwitchTakenPlace();
        serveHolder.GetComponent<FoodPlace>().IsAvailable=false;
        
    }
    void Serve_Enter()
    {
        foodObject.SetActive(true);
        foodObject.transform.position= serveHolder.transform.position;
        serveHolder.GetComponent<FoodPlace>().AddChild(foodObject);
        foodObject=null;
        fsm.ChangeState(States.Idle);
    }
    void Serve_Exit()
    {
        isHolding=false;
        SetAvailable();
        
    }
    public void ReceiveOrder(OrderBoard order, IMachineInterface appropriateMachine)
    {
        currentMenu = order;
        nowUsingMachine= appropriateMachine;
        nowUsingMachine.SwitchTakenPlace();
        placeToMove= ((MonoBehaviour)nowUsingMachine).transform;
        IsAvailable=false;
        fsm.ChangeState(States.Walk);
    }
    public void SetAvailable(){
        IsAvailable=true;
        if(OrderManager.Instance!=null)
            OrderManager.Instance.ChefAvailable();
    }
    public void SetSpeed(float speedMultiplier){
        speed=initSpeed* speedMultiplier;
        agent.speed = speed;
    }
    
}
