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
    public float currentProfitGrowthRate { get{ return characterData.profitGrowthRate[currentLevel - 1]; } }
    public int currentUpgradeMoney { get{return characterData.upgradeMoney[currentLevel - 1]; } }
    public string position;
    public bool isUnlocked{get;private set;}=false;

    public void LevelUp()
    {
        currentLevel++;
        StageMissionManager.Instance.LevelCheck();
        OnProfitChanged?.Invoke(this,currentProfitGrowthRate);
    }
    public void SetData(int level,bool unlock){
        currentLevel=level;
        isUnlocked= unlock;
    }
    public SaveData<Character> GetData()
    {
        SaveData<Character> characterData = new SaveData<Character>(this.characterData.name, currentLevel, isUnlocked);
        return characterData;

    }

}
