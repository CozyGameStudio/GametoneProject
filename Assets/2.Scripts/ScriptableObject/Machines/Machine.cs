using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : MonoBehaviour,IMachineInterface
{
    public bool IsAvailable { get { return !isTakenPlace; } }
    public ScriptableMachine machineData;
    [SerializeField]
    private Food _unlockedFood;
    public bool isUnlocked{get;private set;}=false;
    public List<AdditionalMachine> additionalMachines;
    //public ScriptableFood foodData;
    public bool isTakenPlace{get;private set;}
    
    public int currentLevel{get;private set;}=1;
    public float currentCookTime { get { return machineData.cookTime[currentLevel - 1];} }
    public int currentUpgradeMoney { get {return machineData.upgradeMoney[currentLevel - 1];}}
    public Food unlockedFood
    {
        get { return _unlockedFood; }
        set { _unlockedFood = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        isTakenPlace = false;
    }
    
    public void UnlockFood()
    {
        isUnlocked=true;
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
        StageMissionManager.Instance.LevelCheck();
    }
    public void AddAdditionalMachine(){
        foreach(var additionalMachine in additionalMachines)
        {
            if(!additionalMachine.gameObject.activeInHierarchy){
                Debug.Log("AdditionalMachineAdd");
                additionalMachine.transform.parent.gameObject.SetActive(true);
                additionalMachine.BuyAdditionalMachine();
                return;
            }
        }
    }
    public void SetData(int level,bool unlock)
    {
        currentLevel=level;
        isUnlocked=unlock;
    }
    public SaveData<IMachineInterface> GetData()
    {
 
        return new SaveData<IMachineInterface>(
            this.machineData.name, currentLevel, isUnlocked);
    }
}
