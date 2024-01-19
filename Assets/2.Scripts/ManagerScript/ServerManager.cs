using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerManager : MonoBehaviour
{
    private static ServerManager instance;
    public static ServerManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    public List<GameObject> serveTables;

    private List<GameObject> foodPlaces;

    public List<Server> servers; // all Serves
    private Queue<Transform> serveTasksQueue = new Queue<Transform>();
    private int currentEnabledServer = 1; // it will be controled by data manager
    void Awake(){
        if (null == instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        foreach (var serveTable in serveTables)
        {
            AddChildrenWithName(serveTable, "FoodHolder");
        }
    }
    void Start()
    {
        // every events for foodplace update
        foreach (var foodPlace in foodPlaces)
        {
            foodPlace.GetComponent<FoodPlace>().OnChildAdded += OnChildAddedToFoodPlace;
        }
        foreach (var server in servers)
        {
            server.OnAvailable += OnServerAvailable;

        }
        for (int i = 0; i < servers.Count; i++)
        {
            if (i < currentEnabledServer)
            {
                servers[i].gameObject.SetActive(true);
            }
            else
            {
                servers[i].gameObject.SetActive(false);
            }
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
                Debug.Log(child.name);
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
    public void ActivateOneServer()
    {
        foreach (Server server in servers)
        {
            // check is server disableds
            if (!server.gameObject.activeSelf)
            {
                // if find server to enable, exit the function
                server.gameObject.SetActive(true);
                currentEnabledServer++;
                return; 
            }
        }
    }
}
