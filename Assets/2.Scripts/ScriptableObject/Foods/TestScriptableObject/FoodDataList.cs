using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FoodDataList", menuName = "GametoneProject/FoodDataList", order = 2)]
public class FoodDataList : ScriptableObject
{
    public List<ScriptableFood> foodDataList;
}
