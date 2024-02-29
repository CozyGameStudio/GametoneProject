using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Position", menuName = "GametoneProject/Position")]
public class ScriptablePosition : ScriptableObject
{
    public int index;
    public int stageToUse;
    public int presetNumber;
    public int positionNumber;
    public Sprite icon;
}
