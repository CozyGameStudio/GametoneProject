using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Character : MonoBehaviour
{
    public ScriptableCharacter characterData;
    public delegate void ProfitChangedDelegate(Character cha,float newProfitRate);
    public event ProfitChangedDelegate OnProfitChanged;

    public int currentLevel{get;private set;}=1;
    public float currentProfitGrowthRate { get; private set; }
    public int currentUpgradeMoney { get; private set; }
    public string position;
    public bool isUnlocked{get;private set;}=false;
    public int maxLevel{get;private set;}
    private void Start()
    {
        maxLevel= characterData.profitGrowthRate.Length;
        Debug.Log(maxLevel);
        SetValue(currentLevel);
    }
    public void LevelUp()
    {
        currentLevel++;
        SetValue(currentLevel);
        StageMissionManager.Instance.LevelCheck();
        OnProfitChanged?.Invoke(this,currentProfitGrowthRate);
    }
    void SetValue(int level)
    {
        currentProfitGrowthRate = characterData.profitGrowthRate[level - 1];
        currentUpgradeMoney = characterData.upgradeMoney[level - 1];
    }
}
