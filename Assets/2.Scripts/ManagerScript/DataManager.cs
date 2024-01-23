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

    public int currentActivatedFoods=1;
    public int currentActivatedMachines=1;
    public int currentActivatedCharacters = 1;
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
    public Food RandomFood(){
        List<Food> activeFoods = foods.Where(food => food.gameObject.activeSelf).ToList();

        if (activeFoods.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, activeFoods.Count);
            Food selectedFood = activeFoods[index];
            return selectedFood;
        }
        Debug.Log("no Activated Food");
        return null;
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
    

}
