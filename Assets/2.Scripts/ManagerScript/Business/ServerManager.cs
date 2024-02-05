using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerManager : MonoBehaviour,IManagerInterface
{
    
    public List<GameObject> serveTables;

    private List<GameObject> foodPlaces;

    public List<Server> servers{get;private set;} = new List<Server>(); // all Serves
    private Queue<Transform> serveTasksQueue = new Queue<Transform>();
    public float speedMultiplier = 1.0f; // 기본 속도 계수

    private static ServerManager instance;
    public static ServerManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ServerManager>();
            }
            return instance;
        }
    }
    public void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        Server[] serverObjects = FindObjectsOfType<Server>();
        foreach (Server server in serverObjects)
        {
            servers.Add(server);
        }
        foreach (var serveTable in serveTables)
        {
            AddChildrenWithName(serveTable, "FoodHolder");
        }
        // every events for foodplace update
        foreach (var foodPlace in foodPlaces)
        {
            foodPlace.GetComponent<FoodPlace>().OnChildAdded += OnChildAddedToFoodPlace;
        }
        foreach (var server in servers)
        {
            server.OnAvailable += OnServerAvailable;
        }
        
        
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
    private void OnChildAddedToFoodPlace(Transform child)
    {
        Server availableServer = FindAvailableServer();
        if (availableServer != null)
        {
            availableServer.HandleNewServeTask(child);
        }
        else
        {
            serveTasksQueue.Enqueue(child);
        }
    }
    public void SetSpeedMultiplier(float multiplier)
    {
        speedMultiplier = multiplier;
        foreach (Server server in servers)
        {
            server.MultSpeed(speedMultiplier);
        }
    }
    private Server FindAvailableServer()
    {
        return servers.Find(server => server.IsAvailable);
    }
    public bool isTableFull()
    {
        foreach(var foodPlace in foodPlaces){
            if(foodPlace.GetComponent<FoodPlace>().IsAvailable)return false;
        }
        return true;
    }
    private void OnServerAvailable()
    {
        // if Queue has task
        if (serveTasksQueue.Count > 0)
        {
            Transform task = serveTasksQueue.Dequeue();
            Server availableServer = FindAvailableServer();
            if (availableServer != null)
            {
                availableServer.HandleNewServeTask(task);

            }
        }
    }
    public void SetData(BusinessData data)
    {
        speedMultiplier = data.chefSpeedMultiplier;
    }
    public void AddDataToBusinessData(BusinessData data)
    {
        data.serverSpeedMultiplier = speedMultiplier;
    }

}