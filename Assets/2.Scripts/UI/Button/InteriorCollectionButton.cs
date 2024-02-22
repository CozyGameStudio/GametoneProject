using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteriorCollectionButton : MonoBehaviour
{
    public void TutorialButtonClick()
    {
        if (InteriorSceneManager.Instance.currentInteriorStage == 3)
        {
            if (TutorialManagerForInterior.Instance != null) TutorialManagerForInterior.Instance.InteriorButtonTouch();
        }
    }
}
