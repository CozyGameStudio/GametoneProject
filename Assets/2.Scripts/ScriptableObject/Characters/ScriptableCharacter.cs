using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "GametoneProject/Character", order = 2)]
public class ScriptableCharacter : ScriptableObject
{
    public int index;
    public string characterName;
    public string characterNameInKorean;
    public float[] priceGrowthRate;
    public int[] upgradeMoney;
    public Sprite characterIcon;
}
