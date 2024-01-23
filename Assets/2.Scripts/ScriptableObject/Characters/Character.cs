using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public ScriptableCharacter scriptableCharacter;

    public int currentLevel{get;private set;}=1;
    public float currentPriceGrowthRate { get; private set; }
    public int currentUpgradeMoney { get; private set; }
    private void Start()
    {
        SetValue(currentLevel);
    }
    public void LevelUp()
    {
        currentLevel++;
        SetValue(currentLevel);
        StageMissionManager.Instance.LevelCheck();
    }
    void SetValue(int level)
    {
        currentPriceGrowthRate = scriptableCharacter.priceGrowthRate[level - 1];
        currentUpgradeMoney = scriptableCharacter.upgradeMoney[level - 1];
        Debug.Log(currentPriceGrowthRate + " " + currentUpgradeMoney);
    }
}
