using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MissionDataList", menuName = "GametoneProject/List/MissionDataList", order = 3)]
public class MissionDataList : ScriptableObject
{
    public List<ScriptableMission> missionDataList;

    public void AddMission(ScriptableMission mission)
    {
        if (mission != null && !missionDataList.Contains(mission))
        {
            missionDataList.Add(mission);
        }
    }
    public void cleanList(){
        missionDataList.Clear();
    }
}
