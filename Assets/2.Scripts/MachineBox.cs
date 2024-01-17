using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MachineBox : MonoBehaviour
{
    ScriptableMachine machineData;
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
        if (upgradeCount < machineData.upgradeMoney.Length)
        {
            if (GameManager.Instance.money >= machineData.upgradeMoney[upgradeCount])
            {
                machineUpgradeButton.interactable = true;

            }
            else
            {
                machineUpgradeButton.interactable = false;
            }
        }
    }

    public void InitBox(ScriptableMachine data)
    {
        machineData = data;

        // Init machine image
        Transform machineImageTransform = transform.Find("MachineImage");
        if (machineImageTransform != null)
        {
            machineImage = machineImageTransform.GetComponent<Image>();
            if (machineImage != null)
            {
                machineImage.sprite = machineData.machineIcon;
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
                machineName.text = machineData.machineName;
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
                machineLevel.text = machineData.machineLevel[0].ToString();
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
                CookingSpeed.text = machineData.cookingSpeed[0].ToString();
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
                machineUpgrade.text = machineData.upgradeMoney[0].ToString();
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
        Debug.Log("ButtonClick" + upgradeCount);
        GameManager.Instance.AddMoney(-machineData.upgradeMoney[upgradeCount]);
        upgradeCount++;
        if (upgradeCount == machineData.cookingSpeed.Length)
        {
            machineUpgradeButton.interactable = false;
            machineUpgrade.text = "Max";
        }
        else
        {
            machineUpgrade.text = machineData.upgradeMoney[upgradeCount].ToString();
            CookingSpeed.text = machineData.cookingSpeed[upgradeCount].ToString();
            machineLevel.text = machineData.machineLevel[upgradeCount].ToString();
            
        }
    }
}
