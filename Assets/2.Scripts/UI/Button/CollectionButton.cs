using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionButton : MonoBehaviour
{
    public void TutorialButtonClick()
    {
        if (BusinessGameManager.Instance.currentBusinessStage == 2)
        {
            if (TutorialManagerForStageTwo.Instance != null) TutorialManagerForStageTwo.Instance.CharacterCollectionButtonTouch();
        }
    }
}
