using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
public class DataManager : MonoBehaviour
{
    private static DataManager instance;
    public List<Food> foods{get;private set; } = new List<Food>();
    public List<IMachineInterface> machines { get; private set; } = new List<IMachineInterface>();
    public List<Character> characters { get; private set; } = new List<Character>();

    public List<Food> activeFoods { get; private set; } = new List<Food>();
    public List<IMachineInterface> activeMachines{get;private set;}=new List<IMachineInterface>();
    public List<Character> activeCharacters { get; private set; } = new List<Character>();

    public List<IManagerInterface> managerInterfaces;
    //public List<Character> characters;

    public static DataManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DataManager>();
            }
            return instance;
        }
    }
    public void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    void Start(){
        LoadObjects();
        managerInterfaces = FindObjectsOfType<MonoBehaviour>().OfType<IManagerInterface>().ToList();
    
        //Get Stage Data from loaded data
        StageData data = DataSaveNLoadManager.Instance.GetPreparedData();
        if (data != null)
        {
            if(data.currentStageNumber!= BusinessGameManager.Instance.currentBusinessStage){
                DataSaveNLoadManager.Instance.CreateStageData(BusinessGameManager.Instance.currentBusinessStage);
                data= DataSaveNLoadManager.Instance.LoadStageData();
            }
            foreach(var manager in managerInterfaces){
                manager.SetData(data);
            }
            SetData(data);
        }
        foreach (IMachineInterface machineInterface in machines)
        {
            if(machineInterface is Machine machine&&!machine.isUnlocked)
                machine.transform.parent.gameObject.SetActive(false);
        }
        addActiveFoods();
        addActiveMachines();
        UIManager.Instance.SetData();
    }
    void LoadObjects()
    {
        foods = FindObjectsOfType<Food>().ToList();
        machines = FindObjectsOfType<MonoBehaviour>().OfType<IMachineInterface>().ToList();
        characters = FindObjectsOfType<Character>().ToList();

        foreach (var machineInterface in machines)
        {
            // cast to Machine Type
            if (machineInterface is Machine machine)
            {
                machine.transform.parent.gameObject.SetActive(false);
            }
            // Cast to Additional Machine Type
            else if (machineInterface is AdditionalMachine additionalMachine)
            {
                additionalMachine.transform.parent.gameObject.SetActive(false);
            }
        }
    }
    void addActiveFoods(){
        activeFoods = foods.Where(food => food.isUnlocked).ToList();
    }
    void addActiveMachines()
    {
        activeMachines.Clear(); 

        foreach (IMachineInterface machineInterface in machines)
        {
            if (machineInterface is Machine machine && machine.isUnlocked)
            {
                activeMachines.Add(machine); 
                machine.transform.parent.gameObject.SetActive(true);
            }
            else if (machineInterface is AdditionalMachine additionalMachine && additionalMachine.isUnlocked)
            {
                activeMachines.Add(additionalMachine);
                additionalMachine.transform.parent.gameObject.SetActive(true);
            }
        }

        OrderManager.Instance.MachineListRenew();
    }
    public Food RandomFood(){
        if (activeFoods.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, activeFoods.Count);
            Food selectedFood = activeFoods[index];
            return selectedFood;
        }
        Debug.Log("no Activated Food");
        return null;
    }
    public bool HasUnlockedFood(){
        if (activeFoods.Count > 0)
            return true;
        else
            return false;
    }
    public T FindWithCondition<T>(List<T> items, Predicate<T> match)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (match(items[i]))
            {
                return items[i];
            }
        }
        return default(T);
    }
    public void PurchaseMachine(Machine machineToPurchase)
    {
        if (!machineToPurchase.isUnlocked)
        {
            machineToPurchase.UnlockFood();
            addActiveFoods();
            addActiveMachines();
        }
    }

    public void AddAdditionalMachine(Machine machine)
    {
        machine.AddAdditionalMachine();
        addActiveMachines();
        Debug.Log("DataManager Machine Added");
    }
    public void SetData(StageData data){
        foreach (Food food in foods)
        {
            var foodData = data.currentFoods.Find(f => f.name == food.foodData.name);
            if (foodData != null)
            {
                food.SetData(foodData.currentLevel, foodData.isUnlocked);
            }
        }
        foreach (IMachineInterface machine in machines)
        {
            SaveData<IMachineInterface> machineData = data.currentMachines.Find(m => m.name == machine.GetData().name);
            if (machineData != null)
            {
                machine.SetData(machineData.currentLevel, machineData.isUnlocked);
            }
        }
        foreach (Character character in characters)
        {
            var characterData = data.currentCharacters.Find(f => f.name == character.characterData.name);
            if (characterData != null)
            {
                character.SetData(characterData.currentLevel, characterData.isUnlocked);
            }
        }
    }
    public StageData GetData(){
        //it will Served to DataSaveNLoad Manager
        StageData stageData = new StageData();
        foreach(var manager in managerInterfaces){
            manager.AddDataToStageData(stageData);
        }
        foreach (Food food in foods)
        {
            stageData.currentFoods.Add(food.GetData());
        }
        foreach (IMachineInterface machine in machines)
        {
            stageData.currentMachines.Add(machine.GetData());
        }
        foreach (Character character in characters)
        {
            stageData.currentCharacters.Add(character.GetData());
        }
        return stageData;
    }
    
}
