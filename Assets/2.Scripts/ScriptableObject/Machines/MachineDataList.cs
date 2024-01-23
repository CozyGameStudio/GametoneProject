using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MachineDataList", menuName = "GametoneProject/List/MachineDataList", order = 1)]
public class MachineDataList : ScriptableObject
{
    public List<ScriptableMachine> machineDataList;
}
