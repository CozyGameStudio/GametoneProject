using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditionalMachine : MonoBehaviour,IMachineInterface
{
    public Machine referencedMachine;

    public bool IsAvailable { get { return !isTakenPlace; } }
    public bool isTakenPlace{get;private set;} = false;
    public bool isUnlocked{get;private set;}=false;
    public Food unlockedFood{get{ return referencedMachine != null ? referencedMachine.unlockedFood : null; } }

    public int currentLevel { get { return referencedMachine != null ? referencedMachine.currentLevel : 1; } }
    public float currentCookTime { get { return referencedMachine != null ? referencedMachine.currentCookTime : 5f; } }
    public int currentUpgradeMoney { get { return referencedMachine != null ? referencedMachine.currentUpgradeMoney : 0; } }

    void OnEnable(){
        SetAvailable();
        Debug.Log("AdditionalMachine Enabled");
    }
    public void SwitchTakenPlace()
    {
        isTakenPlace = !isTakenPlace;
        if (isTakenPlace)
        {
            SetAvailable();
        }
    }
    public void SetAvailable()
    {
        if(OrderManager.Instance!=null)
            OrderManager.Instance.MachineAvailable();
    }
    public void BuyAdditionalMachine(){
        isUnlocked=true;
        Debug.Log("unlocked");
    }
    public void SetData(int level, bool unlock)
    {
        isUnlocked = unlock;
    }
    public SaveData<IMachineInterface> GetData()
    {
        SaveData<IMachineInterface> machineData = new SaveData<IMachineInterface>(this.referencedMachine.name, currentLevel, isUnlocked);
        return machineData;
    }
    
}
