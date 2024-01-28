using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.TextCore.Text;

public enum MissionType
{
    Reward,
    Upgrade
}

public enum MissionContent
{
    CustomerCheck,
    SalesCheck,
    LevelCheck,
    ActivatedCheck,
    TableAdd,
    Speedup,
    MachineAdd
}

[CreateAssetMenu(fileName = "NewMission", menuName = "GametoneProject/Mission", order = 4)]
public class ScriptableMission : ScriptableObject
{
    public MissionType missionType;
    public MissionContent missionContent;
    public string targetName;
    public int criteria;
    public int cost;
    public int stageToAppear;
    public Sprite sprite;
    public string description;
}
