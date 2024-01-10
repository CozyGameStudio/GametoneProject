using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public Customer customerPrefab;
    private int n = 0;
    public int createCustomerTime;
    private List<Customer> customers = new List<Customer>();

    void Start()
    {
        StartCoroutine(CreateCustomerEveryFewSeconds());
    }

    IEnumerator CreateCustomerEveryFewSeconds()
    {
        while (true)
        {
            yield return new WaitForSeconds(createCustomerTime);
            if (!CustomerManager.Instance.IsCustomerFull())
            {
                CreateCustomer();
            }
        }

    }

    void CreateCustomer()
    {
        Customer customer = Instantiate(customerPrefab, this.transform.position, this.transform.rotation);
    }
}
