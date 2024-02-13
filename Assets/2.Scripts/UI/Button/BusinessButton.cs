using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusinessButton : MonoBehaviour
{
    public void TutorialButtonClick()
    {
        if (TutorialManager.Instance != null) TutorialManager.Instance.BusinessButtonTouch();
        Debug.Log("Click");
    }
}
