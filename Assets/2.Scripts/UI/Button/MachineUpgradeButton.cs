using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineUpgradeButton : MonoBehaviour
{
    public void TutorialButtonClick()
    {
        TutorialManager.Instance.MachineUpgrade();
    }
}
