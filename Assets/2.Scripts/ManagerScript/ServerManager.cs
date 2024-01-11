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
    public List<FoodPlace> foodPlaces; // 모든 FoodPlace의 리스트
    public List<Server> servers; // 모든 서버의 리스트
    private Queue<Transform> serveTasksQueue = new Queue<Transform>();
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
        // 모든 FoodPlace의 이벤트 구독
        foreach (var foodPlace in foodPlaces)
        {
            foodPlace.OnChildAdded += OnChildAddedToFoodPlace;
        }
        foreach (var server in servers)
        {
            server.OnAvailable += OnServerAvailable;
        }
    }

    private void OnChildAddedToFoodPlace(Transform child)
    {
        //Debug.Log("Event Added");
        // 사용 가능한 서버 찾기
        Server availableServer = FindAvailableServer();
        if (availableServer != null)
        {
            availableServer.HandleNewServeTask(child);
            //Debug.Log("Server Task Added");
        }
        else
        {
            // 사용 가능한 서버가 없다면 큐에 작업 추가
            serveTasksQueue.Enqueue(child);
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
}
