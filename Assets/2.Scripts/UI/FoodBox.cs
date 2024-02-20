using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FoodBox : MonoBehaviour
{
    Food food;
    public Image foodImage;
    public TMP_Text foodName;
    public TMP_Text foodLevel;
    public TMP_Text foodPrice;
    public TMP_Text foodUpgrade;
    public Button foodUpgradeButton;
    public Image foodUpgradeCompleted;
    public GameObject lockPanel;
    public GameObject alarm;
    private void Start(){
        DataLoadManager.Instance.OnDataChanged += UpdateUI;
        BusinessGameManager.Instance.OnCurrencyChangeDelegate += SetAlarm;
    }
    private void HandleFoodUnlocked(Food unlockedFood)
    {
        // Food가 해금될 때 lock panel 비활성화
        if (lockPanel != null)
        {
            lockPanel.SetActive(false);
            SetAlarm();
        }
        if (food != null)
        {
            food.OnFoodUnlocked -= HandleFoodUnlocked;
        }
    }
    public void InitBox(Food foodFromDataManager)
    {
        food = foodFromDataManager;
        if(food.isUnlocked)
            lockPanel.SetActive(!food.isUnlocked);
        else
            food.OnFoodUnlocked += HandleFoodUnlocked;
        // Init food image
        if (foodImage != null)
        {
            foodImage.sprite = food.foodData.foodIcon;
        }
        else
        {
            Debug.LogError("Cannot find Image");
        }

        // Init food name
        if (foodName != null)
        {
            foodName.text = food.foodData.foodNameInKorean;
        }
        else
        {
            Debug.LogError("Cannot find Name");
        }

        // Init food Level
        if (foodLevel != null)
        {
            foodLevel.text = food.currentLevel.ToString();
        }
        else
        {
            Debug.LogError("Cannot find Level");
        }

        // Init food Price
        if (foodPrice != null)
        {
            foodPrice.text = food.currentValue.ToString();
        }
        else
        {
            Debug.LogError("Cannot find foodPrice");
        }

        // Init cooking speed
        if (foodUpgrade != null)
        {
            foodUpgrade.text = food.currentUpgradeMoney.ToString();
        }
        else
        {
            Debug.LogError("Cannot find foodUpgrade");
        }
        UpdateUI();
    }

    public void UpgradeButtonClick()
    {
        
        if (BusinessGameManager.Instance.money < food.currentUpgradeMoney)
        {
            Debug.Log("돈이 읍써여 ㅠㅠㅠㅠ");
            PlaySFXByName("buttonRefuse");
            PlayAnimationByName(foodUpgradeButton.transform,"buttonRefuse");
            return;
        }
        BusinessGameManager.Instance.DecreaseMoney(food.currentUpgradeMoney);
        PlayAnimationByName(foodUpgradeButton.transform, "buttonUpgrade");
        PlaySFXByName("buttonUpgrade");
        food.LevelUp();
        UpdateUI();
    }
    public void UpdateUI(){
        foodUpgrade.text = food.currentUpgradeMoney.ToString();
        foodPrice.text = food.currentValue.ToString();
        foodLevel.text = food.currentLevel.ToString();
        if (food.currentUpgradeMoney == 0)
        {
            foodUpgradeButton.gameObject.SetActive(false);
            foodUpgradeCompleted.gameObject.SetActive(true);
            PlayAnimationByName(foodUpgradeCompleted.transform, "buttonUpgrade");
            alarm.SetActive(false);
        }
    }
    private void PlaySFXByName(string sfxName)
    {
        SystemManager.Instance.PlaySFXByName(sfxName);
    }
    private void PlayAnimationByName(Transform targetTransform,string aniName){
        SystemManager.Instance.PlayAnimationByName(targetTransform,aniName);
    }
    public void SetAlarm()
    {
        if (food.isUnlocked && CheckUpgradable())
        {
            alarm.SetActive(true);
        }
        else
        {
            alarm.SetActive(false);
        }
    }
    private bool CheckUpgradable()
    {
        return food.currentUpgradeMoney <= BusinessGameManager.Instance.money&& !food.currentUpgradeMoney.Equals(0);
    }
}
