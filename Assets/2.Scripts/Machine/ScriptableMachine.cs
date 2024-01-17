using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Machine", menuName = "GametoneProject/Machine", order = 0)]
public class ScriptableMachine : ScriptableObject
{
    public string machineName;
    public int[] machineLevel;
    public float[] cookingSpeed;
    public int[] upgradeMoney;
    public int stage;
    public Sprite machineIcon;
}
