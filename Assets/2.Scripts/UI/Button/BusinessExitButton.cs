using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusinessExitButton : MonoBehaviour
{
    public void TutorialButtonClick()
    {
        if (BusinessGameManager.Instance.currentBusinessStage == 1)
        {
            if (TutorialManager.Instance != null) TutorialManager.Instance.BusinessClose();
        }
    }
}
