using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterButton : MonoBehaviour
{
    public void TutorialButtonClick()
    {
        if (TutorialManagerForStageTwo.Instance != null) TutorialManagerForStageTwo.Instance.CharacterButtonTouch();
        Debug.Log("Click");
    }
}
