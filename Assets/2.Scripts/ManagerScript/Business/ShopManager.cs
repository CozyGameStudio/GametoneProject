using System;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public ShopUI shopUI;
    private int[] jellyReward={2,3,4,5,6,7,10,20};
    public bool[] isRewardReceived{ get; private set; }=new bool[8];
    public int jellyToCoinCount { get; private set; }
    public int coinToJellyCount { get; private set; }
    private int maxCount=3;
    private int currentDayCount=0;
    private int coinAmount=500;
    private int jellyAmount=10;
    private static ShopManager instance;
    public static ShopManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ShopManager>();
            }
            return instance;
        }
    }
    void Start(){
        SetEventToButtons();
        shopUI.InitShopUI(currentDayCount);
    }
    public void CheckDailyReward(SystemData systemData)
    {
        DateTime lastPlayTime;
        if (DateTime.TryParse(systemData.lastTimeStamp, out lastPlayTime))
        {
            lastPlayTime = lastPlayTime.ToLocalTime();
            var currentDate = DateTime.Now.Date;
            var lastPlayDate = lastPlayTime.Date;

            if (currentDate > lastPlayDate)
            {
                currentDayCount++;
                jellyToCoinCount= maxCount;
                coinToJellyCount=maxCount;
            }
        }
        else
        {
            currentDayCount = 1; 
            jellyToCoinCount = maxCount;
            coinToJellyCount = maxCount;
            isRewardReceived = new bool[8];
        }
    }
    public void SetEventToButtons(){
        for(int i=0;i<shopUI.dailyRewardButtons.Count;i++){
            int tmp=i;
            shopUI.dailyRewardButtons[i].button.onClick.AddListener(()=>ReceiveReward(tmp));
        }
    }
    public void ReceiveReward(int i){
        isRewardReceived[i]=true;
        if(DataManager.Instance!=null)StartCoroutine(UIManager.Instance.PlayJellyAttraction(shopUI.dailyRewardButtons[i].button.transform,jellyReward[i]));
        if(i==7){
            currentDayCount=0;
            isRewardReceived=new bool[8];
        }
        shopUI.InitShopUI(currentDayCount);
    }
    public void ChangeCoinToJelly(){
        if(coinToJellyCount<=0|| BusinessGameManager.Instance.money<coinAmount){
            Debug.Log($"[{this.name}] Coin is not Enough for exchange");
            return;
        }
        if(BusinessGameManager.Instance!=null)BusinessGameManager.Instance.DecreaseMoney(coinAmount);
        if(DataManager.Instance!=null)
            StartCoroutine(UIManager.Instance.PlayJellyAttraction(shopUI.coinToJellyButton.transform, jellyAmount));
        coinToJellyCount--;
        shopUI.SetRemainTextForChange();
    }
    public void ChangeJellyToCoin(){
        if (jellyToCoinCount <= 0 || DataManager.Instance.jelly < jellyAmount)
        {
            Debug.Log($"[{this.name}] Jelly is not Enough for exchange");
            SystemManager.Instance.PlaySFXByName("buttonRefuse");
            return;
        }
        if (DataManager.Instance != null) DataManager.Instance.DecreaseJelly(jellyAmount);
        if (BusinessGameManager.Instance != null) 
        StartCoroutine(UIManager.Instance.PlayCoinAttraction(shopUI.jellyToCoinButton.transform, coinAmount));
        jellyToCoinCount--;
        shopUI.SetRemainTextForChange();
    }
    public void SetData(SystemData systemData)
    {
        ShopData shopData=systemData.shopData;
        for(int i=0;i<shopData.isRewardReceived.Length;i++){
            isRewardReceived[i]= shopData.isRewardReceived[i];
        }
        jellyToCoinCount= shopData.jellyToCoinCount;
        coinToJellyCount = shopData.coinToJellyCount;
        currentDayCount=shopData.dayCount;
        CheckDailyReward(systemData);
        shopUI.InitShopUI(currentDayCount);
    }
    public ShopData GetData(){
        ShopData shopData=new ShopData();
        shopData.isRewardReceived=isRewardReceived;
        shopData.jellyToCoinCount = jellyToCoinCount;
        shopData.coinToJellyCount = coinToJellyCount;
        shopData.dayCount=currentDayCount;
        return shopData;
    }
}
