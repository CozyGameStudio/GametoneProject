using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    private static CustomerManager instance;
    public List<GameObject> customerTable;
    public List<GameObject> customerChair{get;private set;}
    public GameObject customerBackPlace;
    public bool[] customerTablePresent{get;private set;}

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
        customerChair=new List<GameObject>();
        foreach (var table in customerTable)
        {
            for(int i=0;i<table.transform.GetChild(0).childCount;i++){
                customerChair.Add(table.transform.GetChild(0).GetChild(i).gameObject);
            }
        }
        // Initialize the numberOfPlaces array according to the number of guest chairs
        int numberOfPlaces = customerChair.Count;
        customerTablePresent = new bool[numberOfPlaces];
        for(int i = 0; i < numberOfPlaces; i++) {
            customerTablePresent[i] = false;
        }

        if(Instance != this) {
            Destroy(gameObject);
        }
    }

    // Determine if the guest chairs are fully occupied
    public bool IsCustomerFull() {
        foreach (var p in customerTablePresent)
        {
            if (p == false)
                return false;
        }
        return true;
    }
}
