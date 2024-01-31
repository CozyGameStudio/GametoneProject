using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Food : MonoBehaviour
{
    public ScriptableFood foodData;

    //later, save & load Manger will change this value
    public int currentLevel{get;private set;}=1;
    public int currentValue { get{return foodData.foodPrice[currentLevel - 1];  }}
    public int currentUpgradeMoney { get{ return foodData.upgradeMoney[currentLevel - 1]; } }
    public bool isUnlocked{get;private set;} // 음식이 해금되었는지 여부

    public event Action<Food> OnFoodUnlocked;

    public void Unlock()
    {
        isUnlocked = true;
        OnFoodUnlocked?.Invoke(this);
    }
    public void LevelUp(){
        currentLevel++;
        StageMissionManager.Instance.LevelCheck();
    }
    public void SetData(int level, bool unlock)
    {
        currentLevel = level;
        isUnlocked = unlock;
    }
    public SaveData<Food> GetData(){
        SaveData<Food> foodData=new SaveData<Food>(this.foodData.name,currentLevel,isUnlocked);
        return foodData;
    }
}
