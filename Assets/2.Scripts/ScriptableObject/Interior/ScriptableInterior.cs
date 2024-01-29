using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Interior", menuName = "GametoneProject/Interior")]
public class ScriptableInterior : ScriptableObject
{
    public int index;
    public string name;
    public string nameInKorean;
    public Sprite icon;
    public int stageToUse;

    public int presetNumber;
    public int position;
}
