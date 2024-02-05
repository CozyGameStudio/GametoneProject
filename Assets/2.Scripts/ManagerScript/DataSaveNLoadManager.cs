using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.SceneManagement;
//using UnityEngine.SceneManagement;
[Serializable]
public class BusinessData
{
    public int currentStageNumber; //현재 플레이중인 경영스테이지 번호
    public int currentStageMoney; //무료재화 정보
    public int currentDia; //유료재화 정보
    public int enabledTables; //활성화 테이블 수
    public float chefSpeedMultiplier; //셰프 속도 증가율
    public float serverSpeedMultiplier; //서버 속도 증가율
    public int accumulatedCustomer;
    public int accumulatedSales;
    public List<SaveData<Food>> currentFoods; //해당 스테이지 음식 데이터
    public List<SaveData<IMachineInterface>> currentMachines; //해당 스테이지 장비 데이터(원본 + 추가 장비)
    public List<SaveData<Character>> currentCharacters; //해당 스테이지 캐릭터 데이터
    public List<MissionData> currentMissions; // 해당스테이지 미션

    public BusinessData(){
        currentStageNumber=1;
        currentStageMoney=100;
        currentDia=0;
        enabledTables=1;
        chefSpeedMultiplier=1;
        serverSpeedMultiplier=1;
        accumulatedCustomer=0;
        accumulatedSales=0;
        currentFoods = new List<SaveData<Food>>();
        currentMachines = new List<SaveData<IMachineInterface>>();
        currentCharacters = new List<SaveData<Character>>();
        currentMissions = new List<MissionData>();
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

public class DataSaveNLoadManager : Singleton<DataSaveNLoadManager>
{
    private int businessStageNumber=1;
    public string sceneName{get;private set;}="";
    private BusinessData loadedData;
    public static Scene scene;
    private void Awake() {
        PrepareData();
        sceneName = "BusinessStage" + loadedData.currentStageNumber.ToString();
        Debug.Log($"{sceneName}Data Load Complete");
        //BusinessStage면, DataManager를 찾아서 Init Setting을 실행
        scene = SceneManager.GetActiveScene();
        Debug.Log($"{scene.name}Scene detected");
        if (scene.name.Contains("Business")){
            DataManager.Instance.DataInitSetting();
        }
    }
    public void CreateBusinessData(int stageNum){
        BusinessData BusinessData = new BusinessData();
        BusinessData.currentStageNumber=stageNum;
        SaveBusinessData(BusinessData);
    }
    public void PrepareData()
    {
        loadedData = LoadBusinessData();
        if (loadedData == null)
        {
            Debug.LogError("No Save Exist - Create New Data");
            CreateBusinessData(1);
            loadedData = LoadBusinessData();
        }
    }

    public BusinessData GetPreparedData()
    {
        return loadedData;
    }
    public int LoadStageNumber()
    {
        string path = Path.Combine(Application.persistentDataPath, "BusinessData.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            BusinessData BusinessData = JsonUtility.FromJson<BusinessData>(json);
            return BusinessData.currentStageNumber;
        }
        else
        {
            return 1;
        }
    }
    public void SaveBusinessData(BusinessData BusinessData)
    {
        string folderPath = Path.Combine(Application.persistentDataPath, "SaveData");
        string filePath = Path.Combine(folderPath, "BusinessData.json");

        // 폴더가 없으면 생성
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string json = JsonUtility.ToJson(BusinessData, true);
        File.WriteAllText(filePath, json);
        Debug.Log("DataSaved!");
    }


    public BusinessData LoadBusinessData()
    {
        string folderPath = Path.Combine(Application.persistentDataPath, "SaveData");
        string filePath = Path.Combine(folderPath, "BusinessData.json");
        if (!Directory.Exists(folderPath)||!File.Exists(filePath))
        {
            return null;
        }
        string json = System.IO.File.ReadAllText(filePath);
        return JsonUtility.FromJson<BusinessData>(json);
    }
    public void SaveGameObjectsFromBusiness()
    {
        BusinessData BusinessData = DataManager.Instance.GetData();
        SaveBusinessData(BusinessData);
    }
    private void OnApplicationPause()
    {
        if(scene.name.Contains("Business")){
            SaveGameObjectsFromBusiness();
        }
    }
    private void OnApplicationQuit() {
        if (scene.name.Contains("Business"))
        {
            SaveGameObjectsFromBusiness();
        }
    }
}


