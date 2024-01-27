using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class DataManager : MonoBehaviour
{
    private static DataManager instance;
    public List<Food> foods;
    public List<Machine> machines;
    public List<Character> characters;

    public List<Food> activeFoods { get; private set; } = new List<Food>();
    public List<Machine> activeMachines{get;private set;}=new List<Machine>();
    public List<Character> activeCharacters { get; private set; } = new List<Character>();

    //public List<Character> characters;

    public static DataManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DataManager>();
            }
            return instance;
        }
    }
    public void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    void Start(){
        foreach (Machine machine in machines)
        {
            machine.transform.parent.gameObject.SetActive(false);
        }
        addActiveFoods();
        addActiveMachines();
    }
    void addActiveFoods(){
        activeFoods = foods.Where(food => food.isUnlocked).ToList();
    }
    void addActiveMachines()
    {
        activeMachines = machines.Where(machine => machine.isPurchased).ToList();
        foreach (Machine activeMachine in activeMachines)
        {
            activeMachine.transform.parent.gameObject.SetActive(true);
        }
        OrderManager.Instance.MachineListRenew();
    }
    public Food RandomFood(){
        if (activeFoods.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, activeFoods.Count);
            Food selectedFood = activeFoods[index];
            return selectedFood;
        }
        Debug.Log("no Activated Food");
        return null;
    }
    public bool HasUnlockedFood(){
        if (activeFoods.Count > 0)
            return true;
        else
            return false;
    }
    public T FindWithCondition<T>(List<T> items, Predicate<T> match)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (match(items[i]))
            {
                return items[i];
            }
        }
        return default(T);
    }
    public void PurchaseMachine(Machine machineToPurchase)
    {
        if (!machineToPurchase.isPurchased)
        {
            machineToPurchase.isPurchased = true;
            machineToPurchase.UnlockFood();
            addActiveFoods();
            addActiveMachines();
        }
        else
        {
            AddAdditionalMachine();
        }
    }

    public void AddAdditionalMachine()
    {
        Debug.Log("Additional machine added: ");
        addActiveMachines();
    }

}
