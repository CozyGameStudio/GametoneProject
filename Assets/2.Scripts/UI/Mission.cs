using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class Mission : MonoBehaviour
{
    public ScriptableMission scriptableMission;
    
    public TMP_Text mission;
    public Image titleImage;
    private string description;
    public Button button{get;private set;}
    public MissionContent missionContent{get;private set;}

    [Header("버튼")]
    public Button upgradeButton;
    public Button rewardButton;

    public object obj{get;private set;}

    public void InitMissionBox()
    {
        missionContent=scriptableMission.missionContent;
        Debug.Log("init mission box started");
        SetButton();
        if (missionContent==MissionContent.LevelCheck||missionContent==MissionContent.ActivatedCheck)
            MatchContentObj();
        SetUI();
        button.gameObject.SetActive(true);
        button.interactable = false;
        titleImage.sprite= scriptableMission.sprite;
    }
    public void SetButton(){
        switch (scriptableMission.missionType)
        {
            case MissionType.Upgrade:
                button = upgradeButton;
                break;
            case MissionType.Reward:
                button = rewardButton;
                break;
        }
    }
    public void MatchContentObj(){
        string target= scriptableMission.targetName;
        if (target.Contains("Machine"))
        {
            Predicate<Machine> machineCondition = (Machine m) => m.machineData.machineName == target;
            obj = DataManager.Instance.FindWithCondition(DataManager.Instance.machines, machineCondition);
        }
        else if (scriptableMission.targetName.Contains("Character"))
        {

        }
        else
        {
            Predicate<Food> foodCondition = (Food f) => f.foodData.foodName == target;
            obj = DataManager.Instance.FindWithCondition(DataManager.Instance.foods, foodCondition);
        }
    }
    public void WhenMachineUpgradeCompleted(Machine machine){
        BusinessGameManager.Instance.DecreaseMoney(scriptableMission.cost);
        StageMissionManager.Instance.CompletedMissionsCount();
        gameObject.SetActive(false);
    }
    public void WhenCharacterUpgradeCompleted(Character character)
    {
        BusinessGameManager.Instance.DecreaseMoney(scriptableMission.cost);
        StageMissionManager.Instance.CompletedMissionsCount();
        gameObject.SetActive(false);
    }
    public void WhenAccCustomerUpgradeCompleted()
    {
        BusinessGameManager.Instance.DecreaseMoney(scriptableMission.cost);
        StageMissionManager.Instance.CompletedMissionsCount();
        gameObject.SetActive(false);
    }
    public void WhenRewardCompleted()
    {
        BusinessGameManager.Instance.AddMoney(scriptableMission.cost);
        StageMissionManager.Instance.CompletedMissionsCount();
        gameObject.SetActive(false);
    }
    public void SetUI()
    {
        string description = "";
        
        //Set description
        switch (missionContent)
        {
            case MissionContent.CustomerCheck:
                description = $"누적손님 {StageMissionManager.Instance.accumulatedCustomer} / {scriptableMission.criteria}";
                break;
            case MissionContent.SalesCheck:
                description = $"누적 판매 금액 {StageMissionManager.Instance.accumulatedSales} / {scriptableMission.criteria}";
                break;
            case MissionContent.LevelCheck:
                if (obj is Food parsedFood)
                {
                    description = $"{parsedFood.foodData.foodNameInKorean} {parsedFood.currentLevel} / {scriptableMission.criteria}";
                }
                else if (obj is Machine parsedMachine)
                {
                    description = $"{parsedMachine.machineData.machineNameInKorean} {parsedMachine.currentLevel} / {scriptableMission.criteria}";
                }
                break;
            case MissionContent.ActivatedCheck:
                if (obj is Machine machine)
                {
                    description = $"{machine.machineData.machineNameInKorean} 설치하기";
                }
                break;
        }
        mission.text = description;
        
    }
    
}
