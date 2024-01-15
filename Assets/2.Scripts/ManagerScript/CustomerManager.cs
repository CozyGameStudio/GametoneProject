using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    private static CustomerManager instance;
    public GameObject[] customerTablePlace;
    public GameObject customerBackPlace;
    public bool[] customerTablePresent;

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
    public void Awake()
    {
        // Initialize the numberOfPlaces array according to the number of guest chairs
        int numberOfPlaces = customerTablePlace.Length;
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
