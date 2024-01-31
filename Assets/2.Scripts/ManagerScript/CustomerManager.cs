using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;

public class CustomerManager : MonoBehaviour,IManagerInterface
{
    private static CustomerManager instance;
    public List<GameObject> customerTable;
    public List<GameObject> customerChair{get;private set;}
    public GameObject customerBackPlace;
    public List<bool> customerChairPresent { get; private set; }

    [HideInInspector]
    public int currentEnabledTable=1;
    
    public static CustomerManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CustomerManager>();
            }
            return instance;
        }
    }
    public void Start()
    {
        
        // Initialize the numberOfPlaces array according to the number of guest chairs

        if(Instance != this) {
            Destroy(gameObject);
        }
    }
    public void AddOneTable(){
        foreach (var table in customerTable)
        {
            // check is server disableds
            if (!table.gameObject.activeSelf)
            {
                // if find server to enable, exit the function
                table.gameObject.SetActive(true);
                currentEnabledTable++;
                AddChairs(table);
                return;
            }
        }
    }
    
    public void AddChairs(GameObject table){
        for (int i = 0; i < table.transform.GetChild(0).childCount; i++)
        {
            GameObject chair = table.transform.GetChild(0).GetChild(i).gameObject;
            if (!customerChair.Contains(chair))
            {
                customerChair.Add(chair);
                customerChairPresent.Add(false);
            }
        }
    }
    // Determine if the guest chairs are fully occupied
    public bool IsCustomerFull() {
        return customerChairPresent.All(p => p);
    }
    public void SetData(BusinessData data){
        currentEnabledTable=data.enabledTables;
        customerChair = new List<GameObject>();
        customerChairPresent = new List<bool>(new bool[customerChair.Count]);
        //enable chef amount of currentEnabledChef
        for (int i = 0; i < customerTable.Count; i++)
        {
            if (i < currentEnabledTable)
            {
                customerTable[i].gameObject.SetActive(true);
                AddChairs(customerTable[i]);
            }
            else
            {
                customerTable[i].gameObject.SetActive(false);
            }
        }
    }
    public void AddDataToStageData(BusinessData data)
    {
        data.enabledTables=currentEnabledTable;
    }
}
