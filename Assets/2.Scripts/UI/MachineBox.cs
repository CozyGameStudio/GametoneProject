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
    public Button machineUpgradeButton;
    int upgradeCount = 0;
    void Start(){
        DataLoadManager.Instance.OnDataChanged += UpdateUI;
    }
    
    private void Update()
    {
        if (upgradeCount < machine.machineData.upgradeMoney.Length)
        {
            if (GameManager.Instance.money >= machine.machineData.upgradeMoney[upgradeCount])
            {
                machineUpgradeButton.interactable = true;

            }
            else
            {
                machineUpgradeButton.interactable = false;
            }
        }
    }

    public void InitBox(Machine machineFromDataManager)
    {
        machine= machineFromDataManager;

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
            machineName.text = machine.machineData.machineName;
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

    }
    public void UpdateUI(){
        machineLevel.text = machine.currentLevel.ToString();
        CookingSpeed.text = machine.currentCookTime.ToString();
        machineUpgrade.text = machine.currentUpgradeMoney.ToString();
    }

    public void UpgradeButtonClick()
    {
        GameManager.Instance.AddMoney(-machine.currentUpgradeMoney);
        machine.LevelUp();
        if (machine.currentUpgradeMoney==0)
        {
            machineUpgradeButton.interactable = false;
            machineUpgrade.text = "Max";
        }
        else
        {
            machineUpgrade.text = machine.currentUpgradeMoney.ToString();
            CookingSpeed.text = machine.currentCookTime.ToString();
            machineLevel.text = machine.currentLevel.ToString();
            
        }
    }
}
