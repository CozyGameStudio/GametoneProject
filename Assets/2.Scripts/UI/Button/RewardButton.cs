using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardButton : MonoBehaviour
{
    public void TutorialButtonClick()
    {
        TutorialManager.Instance.MissionBoxReward();
    }
}
