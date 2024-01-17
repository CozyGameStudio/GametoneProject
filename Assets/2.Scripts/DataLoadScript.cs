using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UGS;
public class DataLoadScript : MonoBehaviour
{
    void Awake()
    {
        //UnityGoogleSheet.LoadAllData();
        Team5DataTable_FoodType.Data.Load();
        // or call DefaultTable.Data.Load(); it's same!
    }

    void Start()
    {
        foreach (var value in Team5DataTable_FoodType.Data.DataList)
        {
            Debug.Log(value.index + "," + value.foodName + "," + value.foodPrice+","+value.cookTime);
        }
        var dataFromMap = Team5DataTable_FoodType.Data.DataMap[0];
        Debug.Log("dataFromMap : " + dataFromMap.index + ", " + dataFromMap.foodName + "," + dataFromMap.foodPrice+","+dataFromMap.cookTime);
    }

}
