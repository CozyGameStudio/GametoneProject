using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
    public Image machineUpgradeComplete;
    public Button machineUpgradeButton;
    public GameObject lockPanel;
    public GameObject alarm;
    void Start(){
        DataLoadManager.Instance.OnDataChanged += UpdateUI;
        BusinessGameManager.Instance.OnCurrencyChangeDelegate+=SetAlarm;
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
        
        
        UpdateUI();
    }
    public void UpgradeButtonClick()
    {
        if (BusinessGameManager.Instance.money < machine.currentUpgradeMoney)
        {
            Debug.Log("돈이 읍써여 ㅠㅠㅠㅠ");
            PlaySFXByName("buttonRefuse");
            PlayAnimationByName(machineUpgradeButton.transform, "buttonRefuse");
            return;
        }
        BusinessGameManager.Instance.DecreaseMoney(machine.currentUpgradeMoney);
        PlaySFXByName("buttonUpgrade");
        PlayAnimationByName(machineUpgradeButton.transform, "buttonUpgrade");
        machine.LevelUp();
        UpdateUI();
    }
    public void UpdateUI()
    {
        machineLevel.text = machine.currentLevel.ToString();
        CookingSpeed.text = machine.currentCookTime.ToString();
        machineUpgrade.text = machine.currentUpgradeMoney.ToString();
        lockPanel.GetComponentInChildren<TMP_Text>().text = $"{machine.machineData.machineUnlockCost}";
        if (machine.currentUpgradeMoney == 0)
        {
            machineUpgradeButton.interactable=false;
            machineUpgradeButton.gameObject.SetActive(false);
            machineUpgradeComplete.gameObject.SetActive(true);
            PlayAnimationByName(machineUpgradeComplete.transform, "buttonUpgrade");
            alarm.SetActive(false);
        }
        SetAlarm();
    }
    public void SetAlarm(){
        if ((machine.isUnlocked && CheckUpgradable()) || (CheckBuyable() && !machine.isUnlocked))
        {
            alarm.SetActive(true);
        }
        else
        {
            alarm.SetActive(false);
        }
    }
    private bool CheckUpgradable(){
        return machine.currentUpgradeMoney <= BusinessGameManager.Instance.money&& !machine.currentUpgradeMoney.Equals(0);
    }
    private bool CheckBuyable(){
        return machine.machineData.machineUnlockCost <= BusinessGameManager.Instance.money;
    }
    public void BuyMachine(){
        if(CheckBuyable())
        {
            DataManager.Instance.PurchaseMachine(machine);
            BusinessGameManager.Instance.DecreaseMoney(machine.machineData.machineUnlockCost);
            StageMissionManager.Instance.ActivatedCheck();
            lockPanel.SetActive(false);
            //연출 부분
            PlaySFXByName("unlock");
            UpdateUI();
        }
        else{
            Debug.Log($"돈이 부족해요....필요한 돈 : {machine.machineData.machineUnlockCost-BusinessGameManager.Instance.money}");
            PlaySFXByName("buttonRefuse");
            PlayAnimationByName(transform, "buttonRefuse");
        }
    }
    private void PlaySFXByName(string sfxName){
        SystemManager.Instance.PlaySFXByName(sfxName);
    }
    private void PlayAnimationByName(Transform targetTransform, string aniName)
    {
        SystemManager.Instance.PlayAnimationByName(targetTransform, aniName);
    }
}
