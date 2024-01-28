using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FoodDataList", menuName = "GametoneProject/List/FoodDataList", order = 0)]
public class FoodDataList : ScriptableObject
{
    public List<ScriptableFood> foodDataList;
    public void AddFood(ScriptableFood food)
    {
        if (food != null && !foodDataList.Contains(food))
        {
            foodDataList.Add(food);
        }
    }
    public void cleanList()
    {
        foodDataList.Clear();
    }
}
