using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UGS;
public class LiveLoadTest : MonoBehaviour
{
    void Start()
    {
        UnityGoogleSheet.LoadFromGoogle<int, Team5DataTable_FoodType.Data>((list, map) =>
        {
            list.ForEach(x =>
            {
                Debug.Log(x.foodPrice);
            });
        }, true);

    }
}
