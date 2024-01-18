using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "GametoneProject/Character", order = 2)]
public class ScriptableCharacter : ScriptableObject
{
    public string characterName;
    public float[] cookingSpeed;
    public int[] upgradeMoney;
    public int stage;
    public Sprite machineIcon;
}
