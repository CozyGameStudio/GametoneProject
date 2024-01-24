using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : MonoBehaviour
{
    public bool IsAvailable { get { return !isTakenPlace; } }
    public ScriptableMachine machineData;
    public Food unlockedFood; 
    public bool isPurchased=false; 

    //public ScriptableFood foodData;
    private bool isTakenPlace = false;
    public int currentLevel{get;private set;}=1;
    public float currentCookTime { get; private set;}
    public int currentUpgradeMoney { get; private set;}
    // Start is called before the first frame update
    void Start()
    {
        isTakenPlace = false;
        SetValue(currentLevel);
    }
    public void UnlockFood()
    {
        if (unlockedFood != null)
        {
            unlockedFood.Unlock();
        }
    }
    public void SwitchTakenPlace()
    {
        isTakenPlace = !isTakenPlace;
        if(isTakenPlace){
            SetAvailable();
        }
    }
    public void SetAvailable()
    {
        OrderManager.Instance.MachineAvailable();
    }
    public void LevelUp()
    {
        currentLevel++;
        SetValue(currentLevel);
        StageMissionManager.Instance.LevelCheck();
    }
    void SetValue(int level)
    {
        currentCookTime = machineData.cookTime[level - 1];
        currentUpgradeMoney = machineData.upgradeMoney[level - 1];
    }
}
