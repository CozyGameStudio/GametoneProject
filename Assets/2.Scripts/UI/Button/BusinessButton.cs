using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusinessButton : MonoBehaviour
{
    public void TutorialButtonClick()
    {
        TutorialManager.Instance.BusinessButtonTouch();
        Debug.Log("Click");
    }
}
