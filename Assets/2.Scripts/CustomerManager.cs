using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    private static CustomerManager instance;
    public GameObject[] customerPlace;
    public bool[] customerPresent;

    /*�̱������� �մԸŴ����� ����*/
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
        int numberOfPlaces = customerPlace.Length;
        customerPresent = new bool[numberOfPlaces];
        for(int i = 0; i < numberOfPlaces; i++) {
            customerPresent[i] = false;
        }

        if(Instance != this) {
            Destroy(gameObject);
        }
    }

    /*�մ� ��Ұ� ����ã���� �Ǵ�*/
    public bool IsCustomerFull() {
        foreach (var p in customerPresent)
        {
            if (p == false)
                return false;
        }
        return true;
    }
}
