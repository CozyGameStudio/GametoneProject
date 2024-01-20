using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Food", menuName = "GametoneProject/Food", order = 0)]
public class ScriptableFood : ScriptableObject
{
    public int index;
    public string foodName;
    public string foodNameInKorean;
    public int[] foodPrice;
    public int[] upgradeMoney;
    public Sprite foodIcon;
    public int stageToUse;
}

