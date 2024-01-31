using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public Customer customerPrefab;
    public int createCustomerTime;
    private List<Customer> customers = new List<Customer>();
    public List<Sprite> spritesForCustomers;
    void Start()
    {
        StartCoroutine(CreateCustomerEveryFewSeconds());
    }

    IEnumerator CreateCustomerEveryFewSeconds()
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

    void CreateCustomer()
    {
        Customer customer = Instantiate(customerPrefab, this.transform.position, this.transform.rotation);
        customer.GetComponent<SpriteRenderer>().sprite= RandomCustomerSprite();
    }
    private Sprite RandomCustomerSprite(){
        if (spritesForCustomers.Count > 0)
        {
            int index = Random.Range(0, spritesForCustomers.Count);
            return spritesForCustomers[index];
        }
        Debug.Log("no Activated Food");
        return null;
    }
}
