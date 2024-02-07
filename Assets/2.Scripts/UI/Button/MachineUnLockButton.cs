using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineUnLockButton : MonoBehaviour
{
    public void TutorialButtonClick()
    {
        TutorialManager.Instance.BusinessMachineUnlock();
    }
}
