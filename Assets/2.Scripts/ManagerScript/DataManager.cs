using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private static DataManager instance;

    public FoodMain[] Food;

    /*�̱������� �����͸Ŵ����� �����ϰ� ĸ��ȭ*/
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
        int index=Random.Range(0,Food.Length);
        Debug.Log(index);
        return Food[index].FoodData.FoodName;
    }
    public FoodMain FindFoodWithName(string foodName){
        for(int i=0;i<Food.Length;i++){
            if(Food[i].FoodData.FoodName.Equals(foodName)){
                return Food[i];
            }
        }
        return null;

    }
}
