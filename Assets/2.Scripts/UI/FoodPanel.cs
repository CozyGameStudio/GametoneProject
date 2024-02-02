using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodPanel : MonoBehaviour
{
    public GameObject BoxPrefab;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < DataManager.Instance.foods.Count; i++)
        {
            GameObject imageObj = Instantiate(BoxPrefab);
            imageObj.transform.SetParent(transform, false);

            FoodBox FoodBox = imageObj.GetComponent<FoodBox>();
            if (FoodBox != null)
            {
                FoodBox.InitBox(DataManager.Instance.foods[i]);
            }
            else
            {
                Debug.LogError("Cannot find FoodBox");
            }
        }
    }
}
