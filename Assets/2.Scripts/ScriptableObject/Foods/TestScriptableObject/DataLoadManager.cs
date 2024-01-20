using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UGS;
public class DataLoadManager : MonoBehaviour
{
    public FoodDataList foodDataList; 
    public TMP_Text[] text;
    //use the data which is saved in local
    public void LoadLocalData(){
        Team5DataTable_Type.FoodTypeData.Load();
        OnFoodTypeDataLoaded(Team5DataTable_Type.FoodTypeData.FoodTypeDataList, Team5DataTable_Type.FoodTypeData.FoodTypeDataMap);
        OnFoodValueDataLoaded(Team5DataTable_Value.FoodValueData.FoodValueDataList, Team5DataTable_Value.FoodValueData.FoodValueDataMap);
        UpdateUI();
    }
    //use the data which is saved in Online
    public void LoadOnlineData()
    {
        Team5DataTable_Type.FoodTypeData.LoadFromGoogle(OnFoodTypeDataLoaded, false);
        Team5DataTable_Value.FoodValueData.LoadFromGoogle(OnFoodValueDataLoaded, false);
        UpdateUI();
    }

    private void OnFoodTypeDataLoaded(List<Team5DataTable_Type.FoodTypeData> loadedDataList, Dictionary<int, Team5DataTable_Type.FoodTypeData> loadedDataMap)
    {
        Dictionary<int, ScriptableFood> existingDataMap = new Dictionary<int, ScriptableFood>();
        foreach (var existingData in foodDataList.foodDataList)
        {
            existingDataMap[existingData.index] = existingData;
        }

        foreach (var loadedData in loadedDataList)
        {
            if (existingDataMap.TryGetValue(loadedData.index, out ScriptableFood existingFoodTypeData))
            {
                existingFoodTypeData.foodName = loadedData.foodName;
                existingFoodTypeData.foodNameInKorean=loadedData.foodNameInKorean;
                existingFoodTypeData.stageToUse = loadedData.stageToUse;
            }
            else
            {
                Debug.Log("not Existing object");
            }
        }
    }
    private void OnFoodValueDataLoaded(List<Team5DataTable_Value.FoodValueData> loadedDataList, Dictionary<string, Team5DataTable_Value.FoodValueData> loadedDataMap)
    {
        Dictionary<string, ScriptableFood> existingDataMap = new Dictionary<string, ScriptableFood>();
        foreach (var existingData in foodDataList.foodDataList)
        {
            existingDataMap[existingData.foodName] = existingData;
        }

        foreach (var loadedData in loadedDataList)
        {
            string[] splitKey = loadedData.foodName.Split('_');
            string foodName = splitKey[0];
            int levelIndex = int.Parse(splitKey[1]) - 1;
            if (existingDataMap.TryGetValue(loadedData.foodName, out ScriptableFood existingFoodTypeData))
            {
                existingFoodTypeData.foodPrice[levelIndex] = loadedData.saleValue;
                existingFoodTypeData.upgradeMoney[levelIndex] = loadedData.upgradeValue;
            }
            else
            {
                Debug.Log("not Existing object");
            }
        }
    }
    public void UpdateUI(){
        for (int i = 0; i < foodDataList.foodDataList.Count; i++)
        {
            if (i < text.Length)
            {
                ScriptableFood data = foodDataList.foodDataList[i];
                text[i].text = data.foodName+" "+data.foodNameInKorean+" "+data.stageToUse;
            }
            else
            {
                break;
            }
        }
    }
}
