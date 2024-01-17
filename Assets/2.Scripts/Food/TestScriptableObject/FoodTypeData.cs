using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Food", menuName = "GametoneProject/FoodTypeTest", order = 1)]
public class FoodTypeData : ScriptableObject
{
    public int index;
    public string foodName;
    public int foodPrice;
    public float cookTime;
    public int stageToUse;
}

