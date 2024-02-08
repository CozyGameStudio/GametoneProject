using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessBarButton : MonoBehaviour
{
    public void TutorialButtonClick()
    {
        TutorialManager.Instance.ProcessBar();
    }
}
