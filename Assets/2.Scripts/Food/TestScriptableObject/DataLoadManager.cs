using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UGS;
public class DataLoadManager : MonoBehaviour
{
    public FoodTypeDataList foodTypeDataList; 
    public TMP_Text[] text;
    void Awake(){
        Team5DataTable_FoodType.Data.Load();
    }
    void Start(){
        LoadOnlineData();
    }
    void Update(){
        UpdateUI();
    }
    public void LoadLocalData(){

    }
    public void LoadOnlineData()
    {
        Team5DataTable_FoodType.Data.LoadFromGoogle(OnFoodTypeDataLoaded, true);
    }

    private void OnFoodTypeDataLoaded(List<Team5DataTable_FoodType.Data> loadedDataList, Dictionary<int, Team5DataTable_FoodType.Data> loadedDataMap)
    {
        Dictionary<int, FoodTypeData> existingDataMap = new Dictionary<int, FoodTypeData>();
        foreach (var existingData in foodTypeDataList.foodTypeDataList)
        {
            existingDataMap[existingData.index] = existingData;
        }

        foreach (var loadedData in loadedDataList)
        {
            if (existingDataMap.TryGetValue(loadedData.index, out FoodTypeData existingFoodTypeData))
            {
                existingFoodTypeData.foodName = loadedData.foodName;
                existingFoodTypeData.foodPrice = loadedData.foodPrice;
                existingFoodTypeData.cookTime = loadedData.cookTime;
                existingFoodTypeData.stageToUse = loadedData.stageToUse;
            }
            else
            {
                Debug.Log("not Existing object");
            }
        }
    }
    public void UpdateUI(){
        for (int i = 0; i < foodTypeDataList.foodTypeDataList.Count; i++)
        {
            if (i < text.Length)
            {
                FoodTypeData data = foodTypeDataList.foodTypeDataList[i];
                text[i].text = $"Name: {data.foodName}, Price: {data.foodPrice}, Cook Time: {data.cookTime}, Stage: {data.stageToUse}";
            }
            else
            {
                break;
            }
        }
    }
}
