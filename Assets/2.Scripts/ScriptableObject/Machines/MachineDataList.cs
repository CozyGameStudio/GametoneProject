using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MachineDataList", menuName = "GametoneProject/List/MachineDataList", order = 1)]
public class MachineDataList : ScriptableObject
{
    public List<ScriptableMachine> machineDataList;
    public void AddMachine(ScriptableMachine machine)
    {
        if (machine != null && !machineDataList.Contains(machine))
        {
            machineDataList.Add(machine);
        }
    }
    public void cleanList()
    {
        machineDataList.Clear();
    }
}
