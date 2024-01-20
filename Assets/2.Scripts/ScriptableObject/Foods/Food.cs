using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField]
    public ScriptableFood foodData;

    //later, save & load Manger will change this value
    public int currentLevel{get;private set;}=1;
    public int currentValue { get; private set; }
    public int currentUpgradeMoney { get; private set; }
    private void Start() {
        SetValue(currentLevel);
    }
    public void LevelUp(){
        currentLevel++;
        SetValue(currentLevel);
    }
    void SetValue(int level){
        currentValue = foodData.foodPrice[level - 1];
        currentUpgradeMoney = foodData.upgradeMoney[level - 1];
        Debug.Log(currentValue+" "+currentUpgradeMoney);
    }
}
