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
    private List<Mission> missions;
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
        WhenAllMissionCompleted.gameObject.SetActive(false);
        Debug.Log("StageMission manager started");
        missions=new List<Mission>();
        for(int i=0;i<missionPanel.transform.childCount;i++){
            missions.Add(missionPanel.transform.GetChild(i).GetComponent<Mission>());
        }
        foreach(var mission in missions){
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
    private void UpdateMissionStatus()
    {
        foreach (var mission in missions)
        {
            switch (mission.missionContent)
            {
                case MissionContent.CustomerCheck:
                    if (accumulatedCustomer >= mission.scriptableMission.criteria)
                    {
                        mission.button.interactable=true;
                    }
                    break;
                case MissionContent.SalesCheck:
                    mission.button.interactable = accumulatedSales >= mission.scriptableMission.criteria;
                    break;
                case MissionContent.LevelCheck:
                    if (mission.obj is Food food)
                    {
                        mission.button.interactable=food.currentLevel>= mission.scriptableMission.criteria;
                    }
                    else if (mission.obj is Machine machine)
                    {
                        mission.button.interactable = machine.currentLevel >= mission.scriptableMission.criteria;
                    }
                    break;
                case MissionContent.ActivatedCheck:
                    if (mission.obj is Machine machineActivatedCheck)
                    {
                        mission.button.interactable = machineActivatedCheck.gameObject.activeInHierarchy;
                    }
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
