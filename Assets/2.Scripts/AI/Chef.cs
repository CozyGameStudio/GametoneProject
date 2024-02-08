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
    public float speed=5f;
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
    private float initSpeed;
    private Animator animator;
    void Awake()
    {
        fsm=new StateMachine<States, StateDriverUnity>(this);
    }
    private void Start() {
        initSpeed=speed;
        fsm.ChangeState(States.Idle);
        serveTables=ServerManager.Instance.serveTables;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        agent.speed=speed;
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
        if (currentVelocity.y > 0.1)
        {//towards upside
            animator.SetFloat("YVelocity", 1);
        }
        else if (currentVelocity.y < -0.1)
        {//towards downward
            animator.SetFloat("YVelocity", -1);
        }
        else
            animator.SetFloat("YVelocity", 0);
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
        SetAnimation(currentVelocity);
    
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
        StartCoroutine(cookCoroutine(foodToMake, nowUsingMachine.currentCookTime));
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
        string foodName= foodToMake.foodData.foodName;
        if (!string.IsNullOrEmpty(foodName) && Char.IsUpper(foodName[foodName.Length - 1]))
        {
            // 마지막 문자가 대문자인 경우, 문자열에서 제거
            foodName = foodName.Substring(0, foodName.Length - 1);
        }
        GameObject foodMade = Instantiate(Resources.Load<GameObject>(foodName), foodHolder.transform.position, Quaternion.identity);
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
}
