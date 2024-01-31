using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MachineBox : MonoBehaviour
{
    Machine machine;
    public Image machineImage;
    public TMP_Text machineName;
    public TMP_Text machineLevel;
    public TMP_Text CookingSpeed;
    public TMP_Text machineUpgrade;
    public TMP_Text machineUpgradeComplete;
    public Button machineUpgradeButton;
    public GameObject lockPanel;
    void Start(){
        DataLoadManager.Instance.OnDataChanged += UpdateUI;
    }

    public void InitBox(Machine machineFromDataManager)
    {
        machine= machineFromDataManager;
        lockPanel.SetActive(!machine.isUnlocked);
        // Init machine image

        if (machineImage != null)
        {
            machineImage.sprite = machine.machineData.machineIcon;
        }
        else
        {
            Debug.LogError("Cannot find Image");
        }

        // Init machine name
        if (machineName != null)
        {
            machineName.text = machine.machineData.machineNameInKorean;
        }
        else
        {
            Debug.LogError("Cannot find Name");
        }

        // Init machine Level
        if (machineLevel != null)
        {
            machineLevel.text = machine.currentLevel.ToString();
        }
        else
        {
            Debug.LogError("Cannot find Level");
        }


        // Init cooking speed
        if (CookingSpeed != null)
        {
            CookingSpeed.text = machine.currentCookTime.ToString();
        }
        else
        {
            Debug.LogError("Cannot find CookingSpeed");
        }


        // Init machine upgrade
        if (machineUpgrade != null)
        {
            machineUpgrade.text = machine.currentUpgradeMoney.ToString();
        }
        else
        {
            Debug.LogError("Cannot find machineUpgrade");
        }
        
        lockPanel.GetComponentInChildren<TMP_Text>().text=$"{machine.machineData.machineUnlockCost}";
        UpdateUI();
    }
    public void UpgradeButtonClick()
    {
        if (BusinessGameManager.Instance.money <= machine.currentUpgradeMoney)
        {
            Debug.Log("돈이 읍써여 ㅠㅠㅠㅠ");
            return;
        }
        BusinessGameManager.Instance.DecreaseMoney(machine.currentUpgradeMoney);
        machine.LevelUp();
        UpdateUI();
    }
    public void UpdateUI()
    {
        machineLevel.text = machine.currentLevel.ToString();
        CookingSpeed.text = machine.currentCookTime.ToString();
        machineUpgrade.text = machine.currentUpgradeMoney.ToString();
        if (machine.currentUpgradeMoney == 0)
        {
            machineUpgradeButton.gameObject.SetActive(false);
            machineUpgradeComplete.gameObject.SetActive(true);
        }
    }
    public void BuyMachine(){
        if(machine.machineData.machineUnlockCost<=BusinessGameManager.Instance.money){
            DataManager.Instance.PurchaseMachine(machine);
            BusinessGameManager.Instance.DecreaseMoney(machine.machineData.machineUnlockCost);
            StageMissionManager.Instance.ActivatedCheck();
            lockPanel.SetActive(false);
        }
        else{
            Debug.Log($"돈이 부족해요....필요한 돈 : {machine.machineData.machineUnlockCost-BusinessGameManager.Instance.money}");
        }
    }
    
}
