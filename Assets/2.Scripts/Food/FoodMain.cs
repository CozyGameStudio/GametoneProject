using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodMain : MonoBehaviour
{
    [SerializeField]
    private ScriptableFood foodData;
    public ScriptableFood FoodData
    {
        get { return foodData; }
        set { foodData = value; }
    }
    public OrderBoard orderstatus;
}
