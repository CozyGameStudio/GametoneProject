using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
public class StageMissionManager : MonoBehaviour
{
    
    public GameObject missionPanel;
    public TMP_Text WhenAllMissionCompleted;
    public int accumulatedCustomer{get;private set;}=0;
    public int accumulatedSales { get; private set; } = 0;
    public int currentCompletedMission { get; private set; } = 0;
    
    private List<ScriptableMission> currentStageMissions = new List<ScriptableMission>();
    private List<MissionBox> missions= new List<MissionBox>();
    private MissionDataList missionDataList;
    private GameObject missionBoxPrefab;

    private static StageMissionManager instance;
    public static StageMissionManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    void Start(){
        missionDataList=Resources.Load<MissionDataList>("MissionDataList");
        missionBoxPrefab=Resources.Load<GameObject>("MissionBox");
        WhenAllMissionCompleted.gameObject.SetActive(false);
        Debug.Log("StageMission manager started");
        foreach(var currentStageMission in missionDataList.missionDataList)
        {
            if(currentStageMission.stageToAppear==BusinessGameManager.Instance.currentBusinessStage){
                currentStageMissions.Add(currentStageMission);
            }
        }
        foreach(var currentStageMission in currentStageMissions)
        {
            GameObject missionBox=Instantiate(missionBoxPrefab);
            missionBox.transform.SetParent(missionPanel.transform, false);
            MissionBox mission= missionBox.GetComponent<MissionBox>();
            mission.missionData= currentStageMission;
            missions.Add(missionBox.GetComponent<MissionBox>());
            mission.InitMissionBox();
        }
        CompletedMissionsCount();
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
    private void UpdateMissionStatus()
    {
        foreach (var mission in missions)
        {
            switch (mission.missionContent)
            {
                case MissionContent.CustomerCheck:
                    if (accumulatedCustomer >= mission.missionData.criteria)
                    {
                        mission.button.interactable=true;
                    }
                    break;
                case MissionContent.SalesCheck:
                    mission.button.interactable = accumulatedSales >= mission.missionData.criteria;
                    break;
                case MissionContent.LevelCheck:
                    if (mission.obj is Food food)
                    {
                        mission.button.interactable=food.currentLevel>= mission.missionData.criteria;
                    }
                    else if (mission.obj is Machine machine)
                    {
                        mission.button.interactable = machine.currentLevel >= mission.missionData.criteria;
                    }
                    break;
                case MissionContent.ActivatedCheck:
                    if (mission.obj is Machine machineActivatedCheck)
                    {
                        mission.button.interactable = machineActivatedCheck.gameObject.activeInHierarchy;
                    }
                    break;
                default :
                    if (BusinessGameManager.Instance.money > mission.missionData.cost)
                    {
                        mission.button.interactable = true;
                    }
                    else mission.button.interactable = false;
                    break;

            }
            mission.SetUI();
        }
    }
    public void CompletedMissionsCount()
    {
        currentCompletedMission = 0;

        foreach (var mission in missions)
        {
            if (!mission.gameObject.activeSelf) 
            {
                currentCompletedMission++;
            }
        }
        Debug.Log(currentCompletedMission);
        if(currentCompletedMission >= missions.Count){
            WhenAllMissionCompleted.gameObject.SetActive(true);
        }

    }
}
