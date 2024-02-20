using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Team5DataTable_Mission;
public class CharacterBox : MonoBehaviour
{
    Character character;
    public Image characterImage;
    public TMP_Text characterName;
    public TMP_Text characterLevel;
    public TMP_Text characterProfitGrowthRate;
    public TMP_Text characterUpgrade;
    public TMP_Text characterUpgradeComplete;
    public Button characterUpgradeButton;
    public Animator animator;
    void Start(){
        DataLoadManager.Instance.OnDataChanged+=UpdateUI;
    }
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
            characterName.text = character.characterData.characterNameInKorean;
        }
        else
        {
            Debug.LogError("Cannot find Name");
        }

        UpdateUI();
    }
    public void UpgradeButtonClick()
    {
        if (DataManager.Instance.jelly <= character.currentUpgradeMoney)
        {
            Debug.Log("돈이 읍써여 ㅠㅠㅠㅠ");
            SystemManager.Instance.PlayAnimationByName(characterUpgradeButton.transform,"buttonRefuse");
            SystemManager.Instance.PlaySFXByName("buttonRefuse");
            return;
        }
        character.LevelUp();
        DataManager.Instance.DecreaseJelly(character.currentUpgradeMoney);
        UpdateUI();
        StartCoroutine(UpgradeAnimation());
        SystemManager.Instance.PlaySFXByName("buttonUpgrade");
    }
    IEnumerator UpgradeAnimation(){
        bool isUpgrade=true;
        animator.SetBool("IsUpgrade", isUpgrade);
        characterUpgradeButton.interactable=false;
        yield return new WaitForSeconds(1);
        isUpgrade = false;
        animator.SetBool("IsUpgrade", isUpgrade); characterUpgradeButton.interactable = true;
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
        else{
            characterUpgradeButton.gameObject.SetActive(true);
            characterUpgradeComplete.gameObject.SetActive(false);
        }
    }
    
}
