using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStoryBuyButton : MonoBehaviour
{
    public void TutorialButtonClick()
    {
        if (BusinessGameManager.Instance.currentBusinessStage == 2)
        {
            if (TutorialManagerForStageTwo.Instance != null) TutorialManagerForStageTwo.Instance.CharacterStoryBuyButtonTouch();
        }
    }
}
