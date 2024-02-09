using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardButton : MonoBehaviour
{
    public void TutorialButtonClick()
    {
        if(BusinessGameManager.Instance.currentBusinessStage == 1)
        {
            TutorialManager.Instance.MissionBoxReward();
        }

    }
}
