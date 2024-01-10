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

    /*�̱������� �մԸŴ����� �����ϰ� ĸ��ȭ*/
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
        /*�մ� ��� ������ ���߾� �մ����翩�� �迭�� �ʱ�ȭ*/
        int numberOfPlaces = customerTablePlace.Length;
        customerTablePresent = new bool[numberOfPlaces];
        for(int i = 0; i < numberOfPlaces; i++) {
            customerTablePresent[i] = false;
        }

        if(Instance != this) {
            Destroy(gameObject);
        }
    }

    /*�մ� ��Ұ� ����ã���� �Ǵ�*/
    public bool IsCustomerFull() {
        foreach (var p in customerTablePresent)
        {
            if (p == false)
                return false;
        }
        return true;
    }
}
