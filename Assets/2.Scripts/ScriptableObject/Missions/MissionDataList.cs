using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MissionDataList", menuName = "GametoneProject/List/MissionDataList", order = 3)]
public class MissionDataList : ScriptableObject
{
    public List<ScriptableMission> missionDataList;
}
