using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Food : MonoBehaviour
{
    public ScriptableFood foodData;

    //later, save & load Manger will change this value
    public int currentLevel{get;private set;}=1;
    public int currentValue { get; private set; }
    public int currentUpgradeMoney { get; private set; }
    public bool isUnlocked{get;private set;} // 음식이 해금되었는지 여부

    public event Action<Food> OnFoodUnlocked;

    public void Unlock()
    {
        isUnlocked = true;
        OnFoodUnlocked?.Invoke(this);
    }
    private void Start() {
        SetValue();
        DataLoadManager.Instance.OnDataChanged += SetValue;
    }
    void OnEnable(){
        SetValue();
    }
    public void LevelUp(){
        currentLevel++;
        SetValue();
        StageMissionManager.Instance.LevelCheck();
    }
    void SetValue(){
        currentValue = foodData.foodPrice[currentLevel - 1];
        currentUpgradeMoney = foodData.upgradeMoney[currentLevel - 1];
    }
}
