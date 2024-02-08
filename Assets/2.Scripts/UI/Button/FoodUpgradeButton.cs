using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodUpgradeButton : MonoBehaviour
{
    public void TutorialButtonClick()
    {
        TutorialManager.Instance.FoodUpgrade();
    }
}
