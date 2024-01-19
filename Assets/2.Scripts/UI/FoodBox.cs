using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FoodBox : MonoBehaviour
{
    Food food;
    Image foodImage;
    TMP_Text foodName;
    TMP_Text foodLevel;
    TMP_Text foodPrice;
    TMP_Text foodUpgrade;
    Button FoodUpgradeButton;
    Transform FoodUpgradeTransform;
    // Start is called before the first frame update
    void Start()
    {
        FoodUpgradeTransform = transform.Find("FoodUpgrade");
        FoodUpgradeButton = FoodUpgradeTransform.GetComponent<Button>();
        if (foodUpgrade == null)
        {
            Debug.LogError("Cannot find machineUpgradeButton");
        }
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
        Transform FoodImageTransform = transform.Find("FoodImage");
        if (FoodImageTransform != null)
        {
            foodImage = FoodImageTransform.GetComponent<Image>();
            if (foodImage != null)
            {
                foodImage.sprite = food.foodData.foodIcon;
            }
            else
            {
                Debug.LogError("Cannot find Image");
            }
        }
        else
        {
            Debug.LogError("Cannot find foodImageTransform");
        }

        // Init food name
        Transform foodNameTransform = transform.Find("FoodName/FoodNameText");
        if (foodNameTransform != null)
        {
            foodName = foodNameTransform.GetComponent<TMP_Text>();
            if (foodName != null)
            {
                foodName.text = food.foodData.foodName;
            }
            else
            {
                Debug.LogError("Cannot find Name");
            }
        }
        else
        {
            Debug.LogError("Cannot find foodNameTextTransform");
        }

        // Init food Level
        Transform foodLevelTransform = transform.Find("FoodLevel/FoodLevelText");
        if (foodLevelTransform != null)
        {
            foodLevel = foodLevelTransform.GetComponent<TMP_Text>();
            if (foodLevel != null)
            {
                foodLevel.text = food.currentLevel.ToString();
            }
            else
            {
                Debug.LogError("Cannot find Level");
            }
        }
        else
        {
            Debug.LogError("Cannot find foodLevelTransform");
        }

        // Init food Price
        Transform FoodPriceTransform = transform.Find("FoodPrice/FoodPriceText");
        if (FoodPriceTransform != null)
        {
            foodPrice = FoodPriceTransform.GetComponent<TMP_Text>();
            if (foodPrice != null)
            {
                foodPrice.text = food.currentValue.ToString();
            }
            else
            {
                Debug.LogError("Cannot find foodPrice");
            }
        }
        else
        {
            Debug.LogError("Cannot find foodPriceTransform");
        }

        // Init cooking speed
        Transform foodUpgradeTransform = transform.Find("FoodUpgrade/FoodUpgradeText");
        if (foodUpgradeTransform != null)
        {
            foodUpgrade = foodUpgradeTransform.GetComponent<TMP_Text>();
            if (foodUpgrade != null)
            {
                foodUpgrade.text = food.currentUpgradeMoney.ToString();
            }
            else
            {
                Debug.LogError("Cannot find foodUpgrade");
            }
        }
        else
        {
            Debug.LogError("Cannot find foodUpgradeTextTransform");
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
