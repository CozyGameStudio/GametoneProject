using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MachineBox : MonoBehaviour
{
    Machine machine;
    Image machineImage;
    TMP_Text machineName;
    TMP_Text machineLevel;
    TMP_Text CookingSpeed;
    TMP_Text machineUpgrade;
    Button machineUpgradeButton;
    Transform machineUpgradeTransform;
    int upgradeCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        machineUpgradeTransform = transform.Find("MachineUpgrade");
        machineUpgradeButton = machineUpgradeTransform.GetComponent<Button>();
        if(machineUpgrade == null)
        {
            Debug.LogError("Cannot find machineUpgradeButton");
        }
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
        Transform machineImageTransform = transform.Find("MachineImage");
        if (machineImageTransform != null)
        {
            machineImage = machineImageTransform.GetComponent<Image>();
            if (machineImage != null)
            {
                machineImage.sprite = machine.machineData.machineIcon;
            }
            else
            {
                Debug.LogError("Cannot find Image");
            }
        }
        else
        {
            Debug.LogError("Cannot find machineImageTransform");
        }

        // Init machine name
        Transform machineNameTransform = transform.Find("MachineName/MachineNameText");
        if (machineNameTransform != null)
        {
            machineName = machineNameTransform.GetComponent<TMP_Text>();
            if (machineName != null)
            {
                machineName.text = machine.machineData.machineName;
            }
            else
            {
                Debug.LogError("Cannot find Name");
            }
        }
        else
        {
            Debug.LogError("Cannot find machineNameTextTransform");
        }

        // Init machine Level
        Transform machineLevelTransform = transform.Find("MachineLevel/MachineLevelText");
        if (machineLevelTransform != null)
        {
            machineLevel = machineLevelTransform.GetComponent<TMP_Text>();
            if (machineLevel != null)
            {
                machineLevel.text = machine.currentLevel.ToString();
            }
            else
            {
                Debug.LogError("Cannot find Level");
            }
        }
        else
        {
            Debug.LogError("Cannot find machineLevelTransform");
        }

        // Init cooking speed
        Transform CookingSpeedTransform = transform.Find("CookingSpeed/CookingSpeedText");
        if (CookingSpeedTransform != null)
        {
            CookingSpeed = CookingSpeedTransform.GetComponent<TMP_Text>();
            if (CookingSpeed != null)
            {
                CookingSpeed.text = machine.currentCookTime.ToString();
            }
            else
            {
                Debug.LogError("Cannot find CookingSpeed");
            }
        }
        else
        {
            Debug.LogError("Cannot find CookingSpeedTransform");
        }

        // Init machine upgrade
        Transform MachineUpgradeTransform = transform.Find("MachineUpgrade/MachineUpgradeText");
        if (MachineUpgradeTransform != null)
        {
            machineUpgrade = MachineUpgradeTransform.GetComponent<TMP_Text>();
            if (machineUpgrade != null)
            {
                machineUpgrade.text = machine.currentUpgradeMoney.ToString();
            }
            else
            {
                Debug.LogError("Cannot find machineUpgrade");
            }
        }
        else
        {
            Debug.LogError("Cannot find machineUpgradeTextTransform");
        }
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
