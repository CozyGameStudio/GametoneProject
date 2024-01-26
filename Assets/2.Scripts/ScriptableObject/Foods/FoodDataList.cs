using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FoodDataList", menuName = "GametoneProject/List/FoodDataList", order = 0)]
public class FoodDataList : ScriptableObject
{
    public List<ScriptableFood> foodDataList;
}
