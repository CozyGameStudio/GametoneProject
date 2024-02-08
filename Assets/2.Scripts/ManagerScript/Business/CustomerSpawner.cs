using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public Customer customerPrefab;
    public int createCustomerTime;
    private List<GameObject> customerPrefabs=new List<GameObject>();
    private void Awake(){
        LoadCustomerPrefabs();
    }
    public IEnumerator CreateCustomerEveryFewSeconds()
    {
        while (true)
        {
            yield return new WaitForSeconds(createCustomerTime);
            if (!CustomerManager.Instance.IsCustomerFull()&&DataManager.Instance.HasUnlockedFood())
            {
                CreateCustomer();
            }
        }
    }
    private void LoadCustomerPrefabs()
    {
        customerPrefabs.Clear();
        GameObject[] loadedPrefabs = Resources.LoadAll<GameObject>("Customers");

        foreach (var prefabGameObject in loadedPrefabs)
        {
            customerPrefabs.Add(prefabGameObject);
        }
    }

    void CreateCustomer()
    {
        if (customerPrefabs.Count > 0)
        {
            int index = Random.Range(0, customerPrefabs.Count);
            GameObject customerPrefab = customerPrefabs[index];
            Instantiate(customerPrefab, this.transform.position, this.transform.rotation);
        }
        else
        {
            Debug.LogError("No customer prefabs loaded.");
        }
    }
    
}
