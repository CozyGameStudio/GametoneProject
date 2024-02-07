using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusinessExitButton : MonoBehaviour
{
    public void TutorialButtonClick()
    {
        TutorialManager.Instance.BusinessClose();
    }
}
