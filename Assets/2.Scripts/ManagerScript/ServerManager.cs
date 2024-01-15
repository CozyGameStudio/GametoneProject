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
    public List<FoodPlace> foodPlaces; // all foodplaces
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
    }
    void Start()
    {
        // every events for foodplace update
        foreach (var foodPlace in foodPlaces)
        {
            foodPlace.OnChildAdded += OnChildAddedToFoodPlace;
        }
        foreach (var server in servers)
        {
            server.OnAvailable += OnServerAvailable;
            Debug.Log($"Subscribed to OnAvailable event of {server.gameObject.name}");
        }
        for (int i = 0; i < servers.Count; i++)
        {
            if (i < currentEnabledServer)
            {
                // Activate the required server
                servers[i].gameObject.SetActive(true);
            }
            else
            {
                // Deactivate unnecessary servers
                servers[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnChildAddedToFoodPlace(Transform child)
    {
        //Debug.Log("Event Added");
        // Find available servers
        Server availableServer = FindAvailableServer();
        if (availableServer != null)
        {
            availableServer.HandleNewServeTask(child);
            //Debug.Log("Server Task Added");
        }
        else
        {
            // If there are no available servers, add the task to the queue
            serveTasksQueue.Enqueue(child);
            Debug.Log(child);
            //Debug.Log("Task added to queue");
        }
    }

    private Server FindAvailableServer()
    {
        return servers.Find(server => server.IsAvailable);
    }
    public bool isTableFull()
    {
        foreach(var foodPlace in foodPlaces){
            if(foodPlace.IsAvailable)return false;
        }
        return true;
    }
    private void OnServerAvailable()
    {
        Debug.Log("OnServerAvailable event triggered.");
        // if Queue has task
        if (serveTasksQueue.Count > 0)
        {
            Transform task = serveTasksQueue.Dequeue();
            Server availableServer = FindAvailableServer();
            if (availableServer != null)
            {
                availableServer.HandleNewServeTask(task);
                //Debug.Log("Server Task Added from queue");
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
                Debug.Log($"{server.gameObject.name} has been activated.");
                return; 
            }
        }

        // if every server is all enabled
        Debug.Log("All servers are already activated.");
    }
}
