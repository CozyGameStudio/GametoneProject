using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FoodTypeDataList", menuName = "GametoneProject/FoodTypeDataList", order = 2)]
public class FoodTypeDataList : ScriptableObject
{
    public List<FoodTypeData> foodTypeDataList;
}
