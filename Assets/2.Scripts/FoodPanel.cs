using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodPanel : MonoBehaviour
{
    public GameObject BoxPrefab;
    private ScriptableFood[] FoodData;
    // Start is called before the first frame update
    void Start()
    {
        FoodData = DataManager.Instance.foodData;
        for (int i = 0; i < FoodData.Length; i++)
        {
            GameObject imageObj = Instantiate(BoxPrefab);
            imageObj.transform.SetParent(transform, false);

            FoodBox FoodBox = imageObj.GetComponent<FoodBox>();
            if (FoodBox != null)
            {
                FoodBox.InitBox(FoodData[i]);
            }
            else
            {
                Debug.LogError("Cannot find FoodBox");
            }
        }
    }
}
