using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CharacterBox : MonoBehaviour
{
    Character character;
    public Image characterImage;
    public TMP_Text characterName;
    public TMP_Text characterLevel;
    public TMP_Text characterProfitGrowthRate;
    public TMP_Text characterPosition;
    public TMP_Text characterUpgrade;
    public TMP_Text characterUpgradeComplete;
    public Button characterUpgradeButton;

    public void InitBox(Character characterFromDataManager)
    {
        character = characterFromDataManager;
        if (characterImage != null)
        {
            characterImage.sprite = character.characterData.characterIcon;
        }
        else
        {
            Debug.LogError("Cannot find Image");
        }

        // Init character name
        if (characterName != null)
        {
            characterName.text = character.characterData.characterName;
        }
        else
        {
            Debug.LogError("Cannot find Name");
        }

        // Init character Level
        if (characterLevel != null)
        {
            characterLevel.text = character.currentLevel.ToString();
        }
        else
        {
            Debug.LogError("Cannot find Level");
        }
        // Init character Level
        if (characterPosition != null)
        {
            characterPosition.text = character.position.ToString();
        }
        else
        {
            Debug.LogError("Cannot find Position");
        }


        // Init cooking speed
        if (characterProfitGrowthRate != null)
        {
            characterProfitGrowthRate.text = character.currentProfitGrowthRate.ToString();
        }
        else
        {
            Debug.LogError("Cannot find CookingSpeed");
        }


        // Init character upgrade
        if (characterUpgrade != null)
        {
            characterUpgrade.text = character.currentUpgradeMoney.ToString();
        }
        else
        {
            Debug.LogError("Cannot find characterUpgrade");
        }
    }
    public void UpgradeButtonClick()
    {
        if (BusinessGameManager.Instance.money <= character.currentUpgradeMoney)
        {
            Debug.Log("돈이 읍써여 ㅠㅠㅠㅠ");
            return;
        }
        character.LevelUp();
        BusinessGameManager.Instance.DecreaseMoney(character.currentUpgradeMoney);
        UpdateUI();
    }
    
    public void UpdateUI()
    {
        characterUpgrade.text = character.currentUpgradeMoney.ToString();
        characterProfitGrowthRate.text = character.currentProfitGrowthRate.ToString();
        characterLevel.text = character.currentLevel.ToString();
        if(character.currentUpgradeMoney==0){
            characterUpgradeButton.gameObject.SetActive(false);
            characterUpgradeComplete.gameObject.SetActive(true);
        }
    }
}
