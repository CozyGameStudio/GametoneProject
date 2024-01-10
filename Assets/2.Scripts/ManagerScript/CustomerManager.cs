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

    /*싱글톤으로 손님매니저를 구성하고 캡슐화*/
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
        /*손님 장소 개수에 맞추어 손님존재여부 배열을 초기화*/
        int numberOfPlaces = customerTablePlace.Length;
        customerTablePresent = new bool[numberOfPlaces];
        for(int i = 0; i < numberOfPlaces; i++) {
            customerTablePresent[i] = false;
        }

        if(Instance != this) {
            Destroy(gameObject);
        }
    }

    /*손님 장소가 가득찾는지 판단*/
    public bool IsCustomerFull() {
        foreach (var p in customerTablePresent)
        {
            if (p == false)
                return false;
        }
        return true;
    }
}
