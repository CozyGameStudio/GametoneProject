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
    private void Start()
    {
        DataLoadManager.Instance.OnDataChanged += SetValue;
        SetValue();
    }
    public void LevelUp()
    {
        currentLevel++;
        SetValue();
        StageMissionManager.Instance.LevelCheck();
        OnProfitChanged?.Invoke(this,currentProfitGrowthRate);
    }
    void SetValue()
    {
        currentProfitGrowthRate = characterData.profitGrowthRate[currentLevel - 1];
        currentUpgradeMoney = characterData.upgradeMoney[currentLevel - 1];
    }
    
}
