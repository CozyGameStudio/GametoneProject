using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Preset", menuName = "GametoneProject/Preset")]
public class ScriptablePreset : ScriptableObject
{
    public int Index;
    public int presetNumber;
    public int stageToUse;
    public int money;
    public Sprite icon;
}
