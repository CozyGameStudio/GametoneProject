using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusinessPanelEmploymentButton : MonoBehaviour
{
    public void TutorialButtonClick()
    {
        if(BusinessGameManager.Instance.currentBusinessStage == 2)
        {
            if(TutorialManagerForStageTwo.Instance != null)
            {
                TutorialManagerForStageTwo.Instance.BusinessPanelEmploymentButtonTouch();
            }
        }
    }
}
