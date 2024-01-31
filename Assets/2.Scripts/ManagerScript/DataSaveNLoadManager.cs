using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEditor.SceneManagement;
[Serializable]
public class BusinessData
{
    public int currentStageNumber; //현재 플레이중인 경영스테이지 번호
    public int currentStageMoney; //무료재화 정보
    public int currentDia; //유료재화 정보
    public int currentProgress; //현재 진척도(미션) 0 ~ 100
    public int enabledTables; //활성화 테이블 수
    public float chefSpeedMultiplier; //셰프 속도 증가율
    public float serverSpeedMultiplier; //서버 속도 증가율
    public List<SaveData<Food>> currentFoods; //해당 스테이지 음식 데이터
    public List<SaveData<IMachineInterface>> currentMachines; //해당 스테이지 장비 데이터(원본 + 추가 장비)
    public List<SaveData<Character>> currentCharacters; //해당 스테이지 캐릭터 데이터
    public List<MissionData> currentMissions; // 해당스테이지 미션

    public BusinessData(){
        currentStageNumber=1;
        currentStageMoney=0;
        currentDia=0;
        currentProgress=0;
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
    public int missionIndex; // 초기값 0
    public bool isUnlocked; //초기값 false
    public bool isCleared; // 초기값 false
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
    public int currentLevel; // 초기값 1
    public bool isUnlocked; // 초기값 false
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
    private BusinessData loadedData;

    private void Awake() {
        loadedData = LoadStageData();
        if (loadedData==null){
            Debug.LogError("No Save Exist");
        }
        sceneName = "BusinessStage" + loadedData.currentStageNumber.ToString();
    }
    public void CreateStageData(int stageNum){
        BusinessData stageData = new BusinessData();
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

    public BusinessData GetPreparedData()
    {
        return loadedData;
    }
    public int LoadStageNumber()
    {
        string path = Path.Combine(Application.persistentDataPath, "stageData.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            BusinessData stageData = JsonUtility.FromJson<BusinessData>(json);
            return stageData.currentStageNumber;
        }
        else
        {
            // 파일이 존재하지 않을 경우, 기본값 반환
            return 1;
        }
    }
    public void SaveStageData(BusinessData stageData)
    {
        string path = Path.Combine(Application.persistentDataPath, "stageData.json");
        string json = JsonUtility.ToJson(stageData,true);
        System.IO.File.WriteAllText(path, json);
    }

    public BusinessData LoadStageData()
    {
        string path = Path.Combine(Application.persistentDataPath, "stageData.json");
        string json = System.IO.File.ReadAllText(path);
        return JsonUtility.FromJson<BusinessData>(json);
    }
    public void SaveGameObjectsFromBusiness()
    {
        BusinessData stageData = DataManager.Instance.GetData();
        SaveStageData(stageData);
    }
    
    private void OnApplicationQuit() {
        //It will be changed after Interior Scene add
        SaveGameObjectsFromBusiness();
    }
}


