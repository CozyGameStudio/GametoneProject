
using System.Collections.Generic;
using UnityEngine;
using System;
class DataLoadRequest
{
    public Action LoadMethod;
    public Action OnComplete;

    public DataLoadRequest(Action loadMethod, Action onComplete)
    {
        this.LoadMethod = loadMethod;
        this.OnComplete = onComplete;
    }
}
public class DataLoadManager : Singleton<DataLoadManager>
{
    public FoodDataList foodDataList; 
    public MachineDataList machineDataList;

    public delegate void DataChangedDelegate();
    public event DataChangedDelegate OnDataChanged;
    private Queue<DataLoadRequest> loadQueue = new Queue<DataLoadRequest>();

    
    private void NotifyDataChanged()
    {
        OnDataChanged?.Invoke();
    }

    //use the data which is saved in local
    public void LoadLocalData(){
        Team5DataTable_Type.FoodTypeData.Load();
        Team5DataTable_Value.FoodValueData.Load();
        Team5DataTable_Type.FoodTypeData.Load();
        Team5DataTable_Value.MachineValueData.Load();
        EnqueueDataLoad(()=> OnFoodTypeDataLoaded(Team5DataTable_Type.FoodTypeData.FoodTypeDataList, Team5DataTable_Type.FoodTypeData.FoodTypeDataMap),OnRequestComplete);
        EnqueueDataLoad(() => OnFoodValueDataLoaded(Team5DataTable_Value.FoodValueData.FoodValueDataList, Team5DataTable_Value.FoodValueData.FoodValueDataMap), OnRequestComplete);
        EnqueueDataLoad(() => OnMachineTypeDataLoaded(Team5DataTable_Type.MachineTypeData.MachineTypeDataList, Team5DataTable_Type.MachineTypeData.MachineTypeDataMap), OnRequestComplete);
        EnqueueDataLoad(() => OnMachineValueDataLoaded(Team5DataTable_Value.MachineValueData.MachineValueDataList, Team5DataTable_Value.MachineValueData.MachineValueDataMap), OnRequestComplete);
    }
    //use the data which is saved in Online
    public void LoadOnlineData()
    {
        EnqueueDataLoad(() => Team5DataTable_Type.FoodTypeData.LoadFromGoogle(OnFoodTypeDataLoaded, false), OnRequestComplete);
        EnqueueDataLoad(() => Team5DataTable_Value.FoodValueData.LoadFromGoogle(OnFoodValueDataLoaded, false), OnRequestComplete);
        EnqueueDataLoad(() => Team5DataTable_Type.MachineTypeData.LoadFromGoogle(OnMachineTypeDataLoaded, false), OnRequestComplete);
        EnqueueDataLoad(() => Team5DataTable_Value.MachineValueData.LoadFromGoogle(OnMachineValueDataLoaded, false), OnRequestComplete);
    }
    private void EnqueueDataLoad(Action loadMethod, Action onComplete)
    {
        loadQueue.Enqueue(new DataLoadRequest(loadMethod, onComplete));

        if (loadQueue.Count == 1)
        {
            ProcessNextRequest();
        }
    }

    private void ProcessNextRequest()
    {
        if (loadQueue.Count > 0)
        {
            var request = loadQueue.Peek();
            request.LoadMethod();
        }
    }

    private void OnRequestComplete()
    {
        loadQueue.Dequeue();

        if (loadQueue.Count == 0)
        {
            Debug.Log("Data All Loaded");
        }
        else
        {
            ProcessNextRequest();
        }
    }
    private void OnMachineTypeDataLoaded(List<Team5DataTable_Type.MachineTypeData> loadedDataList, Dictionary<int, Team5DataTable_Type.MachineTypeData> loadedDataMap)
    {
        Dictionary<int, ScriptableMachine> existingDataMap = new Dictionary<int, ScriptableMachine>();
        foreach (var existingData in machineDataList.machineDataList)
        {
            existingDataMap[existingData.index] = existingData;
        }

        foreach (var loadedData in loadedDataList)
        {
            if (existingDataMap.TryGetValue(loadedData.index, out ScriptableMachine existingMachineTypeData))
            {
                existingMachineTypeData.machineName = loadedData.machineName;
                existingMachineTypeData.machineNameInKorean = loadedData.machineNameInKorean;
                existingMachineTypeData.stageToUse = loadedData.stageToUse;
            }
            else
            {
                Debug.Log("not Existing object");
            }
        }
        OnRequestComplete();
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
        OnRequestComplete();
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
            int levelIndex;
            if (int.TryParse(splitKey[1], out levelIndex) && levelIndex > 0)
            {
                levelIndex -= 1;
                if (existingDataMap.TryGetValue(foodName, out ScriptableFood existingFoodTypeData))
                {
                    if (levelIndex < existingFoodTypeData.foodPrice.Length)
                    {
                        existingFoodTypeData.foodPrice[levelIndex] = loadedData.saleValue;
                        existingFoodTypeData.upgradeMoney[levelIndex] = loadedData.upgradeValue;
                    }
                    else
                    {
                        Debug.LogError($"Level index {levelIndex} is out of range for machine '{foodName}'.");
                    }
                }
                else
                {
                    Debug.Log("Not existing object for machine name: " + foodName);
                }
            }
            else
            {
                Debug.LogError("Failed to parse level index from machine name: " + loadedData.foodName);
            }
        }
        OnRequestComplete();
    }
    private void OnMachineValueDataLoaded(List<Team5DataTable_Value.MachineValueData> loadedDataList, Dictionary<string, Team5DataTable_Value.MachineValueData> loadedDataMap)
    {
        Dictionary<string, ScriptableMachine> existingDataMap = new Dictionary<string, ScriptableMachine>();
        foreach (var existingData in machineDataList.machineDataList)
        {
            existingDataMap[existingData.machineName] = existingData;
        }

        foreach (var loadedData in loadedDataList)
        {
            string[] splitKey = loadedData.machineName.Split('_');
            string machineName = splitKey[0];
            int levelIndex;
            if (int.TryParse(splitKey[1], out levelIndex) && levelIndex > 0)
            {
                levelIndex -= 1;
                if (existingDataMap.TryGetValue(machineName, out ScriptableMachine existingMachineTypeData))
                {
                    if (levelIndex < existingMachineTypeData.cookTime.Length)
                    {
                        existingMachineTypeData.cookTime[levelIndex] = loadedData.cookTime;
                        existingMachineTypeData.upgradeMoney[levelIndex] = loadedData.upgradeValue;
                    }
                    else
                    {
                        Debug.LogError($"Level index {levelIndex} is out of range for machine '{machineName}'.");
                    }
                }
                else
                {
                    Debug.Log("Not existing object for machine name: " + machineName);
                }
            }
            else
            {
                Debug.LogError("Failed to parse level index from machine name: " + loadedData.machineName);
            }
        }
        OnRequestComplete();
    }
}
