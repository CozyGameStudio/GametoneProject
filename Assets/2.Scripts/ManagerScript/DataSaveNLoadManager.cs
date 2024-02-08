using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.SceneManagement;
using System.Linq;
[Serializable]
public class SystemData{
    public BusinessData businessData;
    public List<CollectionData> collectionDatas;
    public SystemData(){
        businessData=new BusinessData();
        collectionDatas=new List<CollectionData>();
    }
}
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
        currentStageMoney=10;
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
public class CollectionData{
    public string name;
    public List<bool> isUnlock;
    public CollectionData(string nam, List<bool> unlockList){
        name=nam;
        isUnlock= unlockList;
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
    private SystemData loadedData;
    public static Scene scene;
    private void Awake() {
        PrepareData();
        //게임 입장시 시작 스테이지 분별을 위한 씬정보 저장
        sceneName = "BusinessStage" + loadedData.businessData.currentStageNumber.ToString();
        Debug.Log($"{sceneName}Data Load Complete");
        scene = SceneManager.GetActiveScene();
        Debug.Log($"{scene.name}Scene detected");
        
        if (scene.name.Contains("Business"))
        {
            DataManager.Instance?.DataInitSetting(loadedData);
            CollectionManager.Instance?.SetData(loadedData);
        }
    }
    public void CreateSystemData(){
        SystemData systemData = new SystemData();
        systemData.businessData = CreateBusinessData(1);
        systemData.collectionDatas = new List<CollectionData>();
        SaveSystemData(systemData);
    }
    public BusinessData CreateBusinessData(int stageNum){
        BusinessData businessData = new BusinessData();
        businessData.currentStageNumber=stageNum;
        return businessData;
    }
    public void PrepareData()
    {
        loadedData = LoadSystemData();
        if (loadedData == null)
        {
            Debug.LogError("No Save Exist - Create New Data");
            CreateSystemData();
            loadedData = LoadSystemData();
        }
    }

    public SystemData GetPreparedData()
    {
        return loadedData;
    }
    
    public void SaveSystemData(SystemData systemData)
    {
        string folderPath = Path.Combine(Application.persistentDataPath, "SaveData");
        string filePath = Path.Combine(folderPath, "SystemData.json");

        // 폴더가 없으면 생성
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string json = JsonUtility.ToJson(systemData, true);
        File.WriteAllText(filePath, json);
        Debug.Log("DataSaved!");
    }
    public SystemData LoadSystemData()
    {
        string folderPath = Path.Combine(Application.persistentDataPath, "SaveData");
        string filePath = Path.Combine(folderPath, "SystemData.json");
        Debug.Log(filePath);
        if (!Directory.Exists(folderPath)||!File.Exists(filePath))
        {
            return null;
        }
        string json = System.IO.File.ReadAllText(filePath);
        return JsonUtility.FromJson<SystemData>(json);
    }
    public void SaveGameObjectsByCase()
    {
        if(DataManager.Instance!=null){
            loadedData.businessData = DataManager.Instance.GetData();
        }
        if (CollectionManager.Instance != null)
        {
            loadedData.collectionDatas=CollectionManager.Instance.GetData();
            Debug.Log("collection added to Data");
        }
        SaveSystemData(loadedData);

    }
    public void ResetData()
    {
        CreateSystemData();
        SceneManager.LoadScene("Title");
    }

    private void OnApplicationPause()
    {
        SaveGameObjectsByCase();
    }
    private void OnApplicationQuit() {
        SaveGameObjectsByCase();
    }
}


