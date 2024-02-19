using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class DataManager : MonoBehaviour
{
    private static DataManager instance;
    public List<Food> foods { get; private set; }=new List<Food>();
    public List<IMachineInterface> machines { get; private set; } = new List<IMachineInterface>();
    public List<Character> characters { get; private set; } = new List<Character>();

    public List<Food> activeFoods { get; private set; } = new List<Food>();
    public List<IMachineInterface> activeMachines{get;private set;}=new List<IMachineInterface>();
    public List<Character> activeCharacters { get; private set; } = new List<Character>();

    public List<IBusinessManagerInterface> managerInterfaces;
    public int jelly { get; private set; } = 0;
    //피버타임 보상 체크를 위한 이벤트 델리게이트
    public delegate void RewardTimeCheckDelegate(float timeLeft);
    public event RewardTimeCheckDelegate OnRewardTimeCheckDelegate;
    public delegate void RewardActivatedDelegate(bool isActivated);
    public event RewardActivatedDelegate OnRewardActivatedDelegate;
    //업그레이드 가능 여부를 감지하기 위한 이벤트
    public delegate void CurrencyChangeDelegate();
    public event CurrencyChangeDelegate OnCurrencyChangeDelegate;
    public bool isRewardActivated { get; private set; }
    public bool isSpeedRewardActivated{get;private set;}
    private int offlineEarningCost=1;
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
    public void DataInitSetting(SystemData systemData)
    {
        Debug.Log("Data Init Start");
        jelly=systemData.currentJelly;
        
        BusinessData data = systemData.businessData;
        if (data!=null){
            Debug.Log("Data Ready");
        }
        LoadObjects();
        managerInterfaces = FindObjectsOfType<MonoBehaviour>().OfType<IBusinessManagerInterface>().ToList();
        foreach (var manager in managerInterfaces)
        {
            Debug.Log($"{manager} ready");
        }
        //Set Stage Data from loaded data
        if (data != null)
        {
            Debug.Log(BusinessGameManager.Instance.currentBusinessStage);
            if(data.currentStageNumber!= BusinessGameManager.Instance.currentBusinessStage){
                DataSaveNLoadManager.Instance.CreateBusinessData(BusinessGameManager.Instance.currentBusinessStage);
                Debug.Log("Create Data Set");
                data = DataSaveNLoadManager.Instance.LoadSystemData().businessData;
                Debug.Log("Load Data Set");
                //Set Start Money for new Stage
                data.currentStageMoney=BusinessGameManager.Instance.startMoney;
                data.accumulatedSales=0;
                data.accumulatedCustomer=0;
                data.enabledTables=2;
                data.chefSpeedMultiplier=1;
                data.serverSpeedMultiplier = 1;
                DataSaveNLoadManager.Instance.businessStageNumber=data.currentStageNumber;
            }
            foreach(var manager in managerInterfaces){
                manager.SetData(data);
                Debug.Log($"{manager} Data Set");
            }
            SetData(data);
        }
        foreach (IMachineInterface machineInterface in machines)
        {
            if(machineInterface is Machine machine&&!machine.isUnlocked)
            {
                machine.transform.parent.gameObject.SetActive(false);
                Debug.Log($"[DataManager] {machine}Create Data Set");
            }
        }
        addActiveFoods();
        addActiveMachines();
        UIManager.Instance.SetData();
        CalculateOfflineEarnings(systemData);
    }
    void LoadObjects()
    {
        //foods = FindObjectsOfType<Food>().ToList();
        var allMachines = FindObjectsOfType<MonoBehaviour>().OfType<IMachineInterface>().ToList();
        //if(allMachines.Count<=0)return;
        var sortedMachines = allMachines
            .OrderBy(machine => machine is Machine ? ((Machine)machine).machineData.machineUnlockCost : int.MaxValue)
            .ToList();
        machines = sortedMachines;
        characters = FindObjectsOfType<Character>().ToList();
        foods= SortFoodsBasedOnMachinesOrder();
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
        Debug.Log("[DataManager] Object Data loaded");
    }
    List<Food> SortFoodsBasedOnMachinesOrder()
    {
        List<Food> sortedFoods = new List<Food>();

        foreach (var machineInterface in machines)
        {
            if (machineInterface is Machine machine && machine.unlockedFood != null && !sortedFoods.Contains(machine.unlockedFood))
            {
                sortedFoods.Add(machine.unlockedFood);
            }
        }
        return sortedFoods;
    }
    void addActiveFoods(){
        activeFoods = foods.Where(food => food.isUnlocked).ToList();
    }
    void addActiveMachines()
    {
        if(machines.Count<=0)return;
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
        Debug.Log("[DataManager] no Activated Food");
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
        Debug.Log("[DataManager] Machine Added");
    }
    public void SetData(BusinessData data){
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
    public BusinessData GetData(){
        //it will Served to DataSaveNLoad Manager
        BusinessData BusinessData = new BusinessData();
        foreach(var manager in managerInterfaces){
            manager.AddDataToBusinessData(BusinessData);
        }
        foreach (Food food in foods)
        {
            BusinessData.currentFoods.Add(food.GetData());
        }
        foreach (IMachineInterface machine in machines)
        {
            BusinessData.currentMachines.Add(machine.GetData());
        }
        foreach (Character character in characters)
        {
            BusinessData.currentCharacters.Add(character.GetData());
        }
        return BusinessData;
    }
    private void CalculateOfflineEarnings(SystemData loadedData)
    {
        if (!string.IsNullOrEmpty(loadedData.lastTimeStamp))
        {
            DateTime lastExitTime = DateTime.Parse(loadedData.lastTimeStamp, null, System.Globalization.DateTimeStyles.RoundtripKind);
            TimeSpan offlineDuration = DateTime.UtcNow - lastExitTime;
            Debug.Log(offlineDuration);
            int earnings=CalculateEarnings(offlineDuration);
            if(!earnings.Equals(0)){
                Debug.Log($"[DataManager] calculate Earning{earnings}");
                OfflineRewardUI offlineRewardUI = null;
                if (UIManager.Instance != null)
                {
                    offlineRewardUI = UIManager.Instance.offlineRewardUI;
                }
                if (offlineRewardUI != null)
                {
                    Debug.Log($"[DataManager] {offlineRewardUI}");
                    offlineRewardUI.SetEarnings(earnings);
                    StartCoroutine(offlineRewardUI.EnterAnimation());
                }
            }
        }
    }
    private int CalculateEarnings(TimeSpan offlineDuration)
    {
        int earnings=0;
        double totalTenMinuteBlocks = Math.Floor(offlineDuration.TotalMinutes / 10);

        if (totalTenMinuteBlocks >= 1 && totalTenMinuteBlocks <= 36) // 최대 6시간==36개의 10분 블록
        {
            earnings = (int)(totalTenMinuteBlocks * offlineEarningCost);
        }
        else if (totalTenMinuteBlocks > 36)
        {
            int maxBlocks = 36; // 최대 6시간에 해당하는 10분 블록
            earnings = maxBlocks * offlineEarningCost;
        }

        return earnings;
    }
    public IEnumerator SetIsRewardActivated(float time)
    {
        isRewardActivated = true;
        float timeLeft = time;
        SystemManager.Instance.PlaySFXByName("fever");
        isSpeedRewardActivated=true;
        OnRewardActivatedDelegate?.Invoke(true);
        while (timeLeft > 0)
        {
            OnRewardTimeCheckDelegate?.Invoke(timeLeft);
            yield return new WaitForSeconds(1f);
            timeLeft -= 1f;
        }
        OnRewardTimeCheckDelegate?.Invoke(0);
        OnRewardActivatedDelegate?.Invoke(false);
        isSpeedRewardActivated=false;
        isRewardActivated = false;
    }
    public void AddJelly(int jellyAmount)
    {
        int tmp=jelly;
        jelly += jellyAmount;
        UIManager.Instance.SetJellyAnimation(tmp,jelly);
        OnCurrencyChangeDelegate?.Invoke();
    }
    public void DecreaseJelly(int jellyAmount)
    {
        if (jellyAmount > jelly) return;
        int tmp=jelly;
        jelly -= jellyAmount;
        UIManager.Instance.SetJellyAnimation(tmp, jelly);
        OnCurrencyChangeDelegate?.Invoke();
    }
    private bool IsCharacterAbleUpgrade(){
        foreach(Character cha in activeCharacters){
            if(!cha.currentUpgradeMoney.Equals(0)&&cha.currentUpgradeMoney<=jelly){
                return true;
            }
        }
        return false;
    }
    private bool IsMachineAbleUpgrade(){
        foreach (var machine in machines)
        {
            if(machine is Machine ma){
                if (!ma.currentUpgradeMoney.Equals(0) && ma.currentUpgradeMoney <= BusinessGameManager.Instance.money)
                {
                    return true;
                }
                if (!ma.isUnlocked && ma.machineData.machineUnlockCost <= BusinessGameManager.Instance.money)
                {
                    return true;
                }
            }
        }
        return false;
    }
    private bool IsFoodAbleUpgrade()
    {
        foreach (Food fo in activeFoods)
        {
            if (!fo.currentUpgradeMoney.Equals(0)&&fo.currentUpgradeMoney <= BusinessGameManager.Instance.money)
            {
                return true;
            }
        }
        return false;
    }
    public bool CheckAbleUpgrade(){
        return IsFoodAbleUpgrade() || IsMachineAbleUpgrade() || IsCharacterAbleUpgrade();
    }
}
