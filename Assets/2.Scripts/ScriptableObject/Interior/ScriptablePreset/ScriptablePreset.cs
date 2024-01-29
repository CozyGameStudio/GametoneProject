using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Preset", menuName = "GametoneProject/Preset")]
public class ScriptablePreset : ScriptableObject
{
    public int number;
    public int money;
    public Sprite icon;
}
