using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEditor.SceneManagement;
[Serializable]
public class StageData
{
    public int currentStageNumber;
    public int currentStageMoney;
    public int currentDia;
    public int currentProgress;
    public int enabledTables;
    public float chefSpeedMultiplier;
    public float serverSpeedMultiplier;
    public List<SaveData<Food>> currentFoods;
    public List<SaveData<IMachineInterface>> currentMachines;
    public List<SaveData<Character>> currentCharacters;
    public List<MissionData> currentMissions;
    
    public StageData(){
        currentStageNumber=1;
        currentStageMoney=0;
        currentDia=0;
        currentFoods=new List<SaveData<Food>>();
        currentMachines=new List<SaveData<IMachineInterface>>();
        currentCharacters=new List<SaveData<Character>>();
        currentMissions=new List<MissionData>();
        enabledTables=1;
        chefSpeedMultiplier=1;
        serverSpeedMultiplier=1;
    }
}
[Serializable]
public class MissionData{
    //Mission
    public int missionIndex;
    public bool isUnlocked;
    public bool isCleared;
    public MissionData(int index,bool unlock,bool clear){
        missionIndex=index;
        isUnlocked=unlock;
        isCleared=clear;
    }
}
[Serializable]
public class SaveData<T>
{
    //Food / Character
    public string name;
    public int currentLevel;
    public bool isUnlocked;
    public SaveData(string nam, int level, bool unlock)
    {
        name = nam;
        currentLevel = level;
        isUnlocked = unlock;
    }
}
// [Serializable]
// public class MachineData<T> where T : IMachineInterface
// {
//     public string name;
//     public int currentLevel;
//     public bool isUnlocked;
//     public List<T> additionalData; // IMachineInterface를 구현하는 타입의 리스트

//     public MachineData(string name, int currentLevel, bool isUnlocked, List<T> additionalData)
//     {
//         this.name = name;
//         this.currentLevel = currentLevel;
//         this.isUnlocked = isUnlocked;
//         this.additionalData = additionalData ?? new List<T>();
//     }

//     public MachineData(string name, int currentLevel, bool isUnlocked)
//     {
//         this.name = name;
//         this.currentLevel = currentLevel;
//         this.isUnlocked = isUnlocked;
//     }
// }


public class DataSaveNLoadManager : Singleton<DataSaveNLoadManager>
{
    private int businessStageNumber=1;
    public string sceneName{get;private set;}="";
    private StageData loadedData;

    private void Awake() {
        loadedData = LoadStageData();
        if (loadedData==null){
            Debug.LogError("No Save Exist");
        }
        sceneName = "BusinessStage" + loadedData.currentStageNumber.ToString();
    }
    public void CreateStageData(int stageNum){
        StageData stageData = new StageData();
        SaveStageData(stageData);
    }
    public void PrepareDataForNextScene()
    {
        loadedData = LoadStageData();
        if (loadedData == null)
        {
            Debug.LogError("No Save Exist");
        }
    }

    public StageData GetPreparedData()
    {
        return loadedData;
    }
    public int LoadStageNumber()
    {
        string path = Path.Combine(Application.persistentDataPath, "stageData.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            StageData stageData = JsonUtility.FromJson<StageData>(json);
            return stageData.currentStageNumber;
        }
        else
        {
            // 파일이 존재하지 않을 경우, 기본값 반환
            return 1;
        }
    }
    public void SaveStageData(StageData stageData)
    {
        string path = Path.Combine(Application.persistentDataPath, "stageData.json");
        string json = JsonUtility.ToJson(stageData,true);
        System.IO.File.WriteAllText(path, json);
    }

    public StageData LoadStageData()
    {
        string path = Path.Combine(Application.persistentDataPath, "stageData.json");
        string json = System.IO.File.ReadAllText(path);
        return JsonUtility.FromJson<StageData>(json);
    }
    public void SaveGameObjectsFromBusiness()
    {
        StageData stageData = DataManager.Instance.GetData();
        SaveStageData(stageData);
    }
    
    private void OnApplicationQuit() {
        //It will be changed after Interior Scene add
        SaveGameObjectsFromBusiness();
    }
}


