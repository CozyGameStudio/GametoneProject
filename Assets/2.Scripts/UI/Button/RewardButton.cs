using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardButton : MonoBehaviour
{
    public Image currencySprite;
    public void TutorialButtonClick()
    {
        if(BusinessGameManager.Instance.currentBusinessStage == 1)
        {
            if(TutorialManager.Instance!=null) TutorialManager.Instance.MissionBoxReward();
        }

    }
}
