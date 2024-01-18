using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private static DataManager instance;
    public List<Food> foods;
    public List<Machine> machines;
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
    public string RandomFood(){
        int index=Random.Range(0,foods.Count);
        return foods[index].foodData.foodName;
    }
    public Food FindFoodWithName(string foodName){
        for(int i=0;i<foods.Count;i++){
            if(foods[i].foodData.foodName.Equals(foodName)){
                return foods[i];
            }
        }
        return null;
    }
}
