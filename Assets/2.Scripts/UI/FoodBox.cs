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
    public Button FoodUpgradeButton;
    // Start is called before the first frame update
    void Start()
    {
    }

    private void Update()
    {
        if (GameManager.Instance.money >= food.currentUpgradeMoney)
        {
            FoodUpgradeButton.interactable = true;

        }
        else
        {
            FoodUpgradeButton.interactable = false;
        }
    }

    public void InitBox(Food foodFromDataManager)
    {
        food = foodFromDataManager;

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
            foodName.text = food.foodData.foodName;
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
    }

    public void UpgradeButtonClick()
    {
        GameManager.Instance.DecreaseMoney(food.currentUpgradeMoney);
        food.LevelUp();
        if (food.currentUpgradeMoney == 0)
        {
            FoodUpgradeButton.interactable = false;
            foodUpgrade.text = "Max";
        }
        else
        {
            SetText();
        }
    }
    public void SetText(){
        foodUpgrade.text = food.currentUpgradeMoney.ToString();
        foodPrice.text = food.currentValue.ToString();
        foodLevel.text = food.currentLevel.ToString();
    }
}
