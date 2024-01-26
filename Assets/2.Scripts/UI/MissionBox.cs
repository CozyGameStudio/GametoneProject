using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class MissionBox : MonoBehaviour
{
    public ScriptableMission missionData;
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
        missionContent=missionData.missionContent;
        Debug.Log("init mission box started");
        SetButton();
        if (missionContent==MissionContent.LevelCheck||missionContent==MissionContent.ActivatedCheck)
            MatchContentObj();
        SetUI();
        button.gameObject.SetActive(true);
        button.interactable = false;
        titleImage.sprite= missionData.sprite;
    }
    public void SetButton(){
        switch (missionData.missionType)
        {
            case MissionType.Upgrade:
                button = upgradeButton;
                if(missionData.missionContent==MissionContent.TableAdd){
                    button.onClick.AddListener(TableUpgrade);
                }
                else if(missionData.missionContent == MissionContent.Speedup)
                {
                    button.onClick.AddListener(SpeedUpgrade);
                }
                else if(missionData.missionContent == MissionContent.MachineAdd)
                {
                    button.onClick.AddListener(MachineUpgrade);
                }
                break;
            case MissionType.Reward:
                button = rewardButton;
                button.onClick.AddListener(Reward);
                break;
        }
    }
    public void MatchContentObj(){
        string target= missionData.targetName;
        if (target.Contains("Machine"))
        {
            Predicate<Machine> machineCondition = (Machine m) => m.machineData.machineName == target;
            obj = DataManager.Instance.FindWithCondition(DataManager.Instance.machines, machineCondition);
        }
        else if (missionData.targetName.Contains("Character"))
        {
            // Predicate<Character> characterCondition = (Character c) => c.characterData.characterName == target;
            // obj = DataManager.Instance.FindWithCondition(DataManager.Instance.characters, characterCondition);
        }
        else
        {
            Predicate<Food> foodCondition = (Food f) => f.foodData.foodName == target;
            obj = DataManager.Instance.FindWithCondition(DataManager.Instance.foods, foodCondition);
        }
        Debug.Log(obj.ToString());
    }
    public void MachineUpgrade(){
        BusinessGameManager.Instance.DecreaseMoney(missionData.cost);
        StageMissionManager.Instance.CompletedMissionsCount();
        DataManager.Instance.AddAdditionalMachine();
        gameObject.SetActive(false);
    }
    public void SpeedUpgrade()
    {
        BusinessGameManager.Instance.DecreaseMoney(missionData.cost);
        StageMissionManager.Instance.CompletedMissionsCount();
        foreach (Chef chef in OrderManager.Instance.chefs)
        {
            if(chef.gameObject.activeSelf)
                chef.MultSpeed(1.4f);
        }
        foreach (Server server in ServerManager.Instance.servers)
        {
            if (server.gameObject.activeSelf)
                server.MultSpeed(1.4f);
        }
        gameObject.SetActive(false);
    }
    public void TableUpgrade()
    {
        BusinessGameManager.Instance.DecreaseMoney(missionData.cost);
        StageMissionManager.Instance.CompletedMissionsCount();
        CustomerManager.Instance.AddOneTable();
        gameObject.SetActive(false);
    }
    public void Reward()
    {
        BusinessGameManager.Instance.AddMoney(missionData.cost);
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
                description = $"누적손님 {StageMissionManager.Instance.accumulatedCustomer} / {missionData.criteria}";
                break;
            case MissionContent.SalesCheck:
                description = $"누적 판매 금액 {StageMissionManager.Instance.accumulatedSales} / {missionData.criteria}";
                break;
            case MissionContent.LevelCheck:
                if (obj is Food parsedFood)
                {
                    description = $"{parsedFood.foodData.foodNameInKorean} {missionData.criteria}LV 달성하기";
                    Debug.Log("In");
                }
                else if (obj is Machine parsedMachine)
                {
                    description = $"{parsedMachine.machineData.machineNameInKorean} {missionData.criteria}LV 달성하기";
                    Debug.Log("In");
                }
                break;
            case MissionContent.ActivatedCheck:
                if (obj is Machine machine)
                {
                    description = $"{machine.machineData.machineNameInKorean} 설치하기";
                    Debug.Log("In");
                }
                break;
            case MissionContent.TableAdd:
                description = "테이블 설치하기";
                break;
            case MissionContent.MachineAdd:
                description = "기계 설치하기";
                break;
            case MissionContent.Speedup:
                description = "직원 속도 2배로 올리기";
                break;

        }
        mission.text = description;
    }
    
}
