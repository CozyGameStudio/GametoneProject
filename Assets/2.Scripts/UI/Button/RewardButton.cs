using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardButton : MonoBehaviour
{
    public void TutorialButtonClick()
    {
        if(TutorialManager.Instance!=null)TutorialManager.Instance.MissionBoxReward();
    }
}
