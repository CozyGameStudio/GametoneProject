using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Machine", menuName = "GametoneProject/Machine", order = 0)]
public class ScriptableMachine : ScriptableObject
{
    public int index;
    public string machineName;
    public string machineNameInKorean;
    public int machineUnlockCost;
    public float[] cookTime;
    public int[] upgradeMoney;
    public int stageToUse;
    public Sprite machineIcon;
}
