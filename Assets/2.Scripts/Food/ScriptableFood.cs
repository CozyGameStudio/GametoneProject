using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Food", menuName = "GametoneProject/Food", order = 0)]
public class ScriptableFood : ScriptableObject
{
    public string foodName;
    public int[] foodPrice;
    public int[] upgradeMoney;
    public Sprite foodIcon;
    public GameObject foodPrefab;
    public int[] foodLevel;
    public int stage;
}

