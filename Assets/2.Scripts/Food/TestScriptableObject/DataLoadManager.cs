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
        Team5DataTable_FoodType.Data.LoadFromGoogle(OnDataLoaded, true);
    }

    private void OnDataLoaded(List<Team5DataTable_FoodType.Data> loadedDataList, Dictionary<int, Team5DataTable_FoodType.Data> loadedDataMap)
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
                // 기존 데이터 업데이트
                existingFoodTypeData.foodName = loadedData.foodName;
                existingFoodTypeData.foodPrice = loadedData.foodPrice;
                existingFoodTypeData.cookTime = loadedData.cookTime;
                existingFoodTypeData.stageToUse = loadedData.stageToUse;
            }
            else
            {
                // 새 데이터 추가
                FoodTypeData newFoodTypeData = new FoodTypeData
                {
                    index = loadedData.index,
                    foodName = loadedData.foodName,
                    foodPrice = loadedData.foodPrice,
                    cookTime = loadedData.cookTime,
                    stageToUse = loadedData.stageToUse
                };

                foodTypeDataList.foodTypeDataList.Add(newFoodTypeData);
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
