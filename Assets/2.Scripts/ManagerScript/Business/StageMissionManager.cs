using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
public class StageMissionManager : MonoBehaviour,IBusinessManagerInterface
{
    
    public GameObject missionPanel;
    public TMP_Text WhenAllMissionCompleted;
    public int accumulatedCustomer{get;private set;}=0;
    public int accumulatedSales { get; private set; } = 0;
    public int currentCompletedMission { get; private set; } = 0;
    
    private List<ScriptableMission> currentStageMissions = new List<ScriptableMission>();
    public List<MissionBox> missions{get;private set;}= new List<MissionBox>();
    private MissionDataList missionDataList;
    private GameObject missionBoxPrefab;
    public int stageProgress{get;private set;}=0;
    public bool isRewardAble{get;private set;}=false;
    public delegate void StageClearedDelegate();
    public event StageClearedDelegate OnStageCleared;
    //리워드 가능 여부를 감지하기 위한 이벤트
    public delegate void RewardAbleDelegate(bool isAble);
    public event RewardAbleDelegate OnRewardAbleDelegate;
    private static StageMissionManager instance;
    public static StageMissionManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<StageMissionManager>();
            }
            return instance;
        }
    }
    public void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        DataLoadManager.Instance.OnDataChanged += UpdateMissionStatus;
    }
    public void CalculateProgress(){
        stageProgress = 0;

        foreach (var mission in missions)
        {
            if (mission.isCleared)
            {
                stageProgress += mission.missionData.progress;
            }
        }
        Debug.Log($"{name} Stage Progress : {stageProgress}");
        Debug.Log(currentCompletedMission);
        if (stageProgress == 100)
        {
            Debug.Log("Stage Clear!!!!");
            UIManager.Instance.MissionWindowOff();
            TimelineManager.Instance.PlayCutScene();
        }
        UIManager.Instance.UpdateProgress();
    }
    public void MissionInit(ScriptableMission currentStageMission)
    {
        Debug.Log("MIssion Box Created");
        GameObject missionBox = Instantiate(missionBoxPrefab);
        missionBox.transform.SetParent(missionPanel.transform, false);
        MissionBox mission = missionBox.GetComponent<MissionBox>();
        mission.missionData = currentStageMission;
        missions.Add(missionBox.GetComponent<MissionBox>());
        mission.InitMissionBox();
    }
    public void IncreaseAccumulatedCustomer(){
        accumulatedCustomer++;
        UpdateMissionStatus();
    }
    public void IncreaseAccumulatedSales(int money){
        accumulatedSales+=money;
        UpdateMissionStatus();
    }
    public void LevelCheck(){
        UpdateMissionStatus();
    }
    public void ActivatedCheck(){
        UpdateMissionStatus();
    }
    public void CostCheck(){
        UpdateMissionStatus();
    }
    public void CallUpdateMissionStatus(){
        UpdateMissionStatus();
    }
    private void UpdateMissionStatus()
    {
        int missionClearNumberForTutorial=0;
        isRewardAble=false;
        foreach (var mission in missions)
        {
            if(mission.isCleared)continue;
            switch (mission.missionContent)
            {
                case MissionContent.CustomerCheck:
                    if (accumulatedCustomer >= mission.missionData.criteria)
                    {
                        mission.button.interactable=true;
                        isRewardAble=true;
                    }
                    break;
                case MissionContent.SalesCheck:
                    if(accumulatedSales >= mission.missionData.criteria){
                        mission.button.interactable =true;
                        missionClearNumberForTutorial++;
                        isRewardAble = true;
                    }
                    break;
                case MissionContent.LevelCheck:
                    if (mission.obj is Food food)
                    {
                        if(food.currentLevel >= mission.missionData.criteria){
                            mission.button.interactable = true;
                            isRewardAble = true;
                        }
                        
                    }
                    else if (mission.obj is Machine machine)
                    {
                        if(machine.currentLevel >= mission.missionData.criteria){
                            mission.button.interactable = true;
                            missionClearNumberForTutorial++;
                            isRewardAble = true;
                        }
                    }
                    break;
                case MissionContent.ActivatedCheck:
                    if (mission.obj is Machine machineActivatedCheck)
                    {
                        if(machineActivatedCheck.gameObject.activeInHierarchy){
                            mission.button.interactable = true;
                            isRewardAble = true;
                        }
                        
                    }
                    break;
                case MissionContent.MachineNumberCheck:
                    if (DataManager.Instance.activeMachines.Count >= mission.missionData.criteria)
                    {
                        mission.button.interactable = true;
                        isRewardAble = true;
                    }
                    break;
                default :
                    if (BusinessGameManager.Instance.money >= mission.missionData.cost&&mission.isUnlocked)
                    {
                        mission.button.interactable = true;
                        isRewardAble = true;
                    }
                    else mission.button.interactable = false;
                    break;
            }
            mission.SetUI();
            mission.SetActiveByStatus();
        }
        OnRewardAbleDelegate?.Invoke(isRewardAble);//미션 상태 추적이 완료되었음을 알리는 이벤트
        if (TutorialManager.Instance != null && missionClearNumberForTutorial == 3) EventDispatcher.MissionCompleted();
    }
    public void SetData(BusinessData data)
    {
        missionDataList = Resources.Load<MissionDataList>("MissionDataList");
        missionBoxPrefab = Resources.Load<GameObject>("MissionBox");
        //WhenAllMissionCompleted.gameObject.SetActive(false);
        
        Debug.Log("StageMission manager started");
        foreach (var currentStageMission in missionDataList.missionDataList)
        {
            if (currentStageMission.stageToAppear == BusinessGameManager.Instance.currentBusinessStage)
            {
                currentStageMissions.Add(currentStageMission);
                MissionInit(currentStageMission);
            }
        }
        
        foreach (var missionData in data.currentMissions)
        {
            var missionBox = missions.Find(m => m.missionData.index == missionData.missionIndex);
            if (missionBox != null)
            {
                missionBox.SetData(missionData.isUnlocked, missionData.isCleared);
                missionBox.CheckClearNUnlockStatus();
            }
        }
        accumulatedCustomer = data.accumulatedCustomer;
        accumulatedSales = data.accumulatedSales;
        UpdateMissionStatus();
        CalculateProgress();
    }
    public void AddDataToBusinessData(BusinessData data)
    {
        List <MissionData> tmpList=new List<MissionData>();
        foreach (var mission in missions){
            tmpList.Add(mission.GetData());
        }
        data.accumulatedCustomer = accumulatedCustomer;
        data.accumulatedSales = accumulatedSales;
        data.currentMissions=tmpList;
        
    }
    public void TriggerStageCleared()
    {
        OnStageCleared?.Invoke();
    }
}
