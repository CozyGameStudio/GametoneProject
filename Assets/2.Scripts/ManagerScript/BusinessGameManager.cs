using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BusinessGameManager : MonoBehaviour,IManagerInterface
{
    private Dictionary<Character, float> characterProfits = new Dictionary<Character, float>();
    private static BusinessGameManager instance;
    public int money{get;private set;}=0;
    public int dia{get;private set;}=0;
    public float moneyMultiplier=1;

    public int currentBusinessStage=1;
    public static BusinessGameManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        Input.multiTouchEnabled=false;
        UIManager.Instance.UpdateMoneyUI();
    }
    

    void Start()
    {
        
            
#if UNITY_ANDROID
        Application.targetFrameRate=60;
#else
        QualitySettings.vSyncCount=1;
#endif
        // 모든 캐릭터에 대해 이벤트 구독
        foreach (var character in FindObjectsOfType<Character>())
        {
            character.OnProfitChanged += HandleProfitChange;
            characterProfits[character] = character.currentProfitGrowthRate;
        }
        RecalculateMoneyMultiplier();
    }

    private void HandleProfitChange(Character character, float newProfitRate)
    {
        if (characterProfits.ContainsKey(character))
        {
            characterProfits[character] = newProfitRate;
        }
        else
        {
            characterProfits.Add(character, newProfitRate);
        }

        RecalculateMoneyMultiplier();
    }

    private void RecalculateMoneyMultiplier()
    {
        moneyMultiplier = 1;
        foreach (var profit in characterProfits.Values)
        {
            moneyMultiplier += profit; 
        }
        UIManager.Instance.UpdateMoneyMultiplierUI();
    }
    public void AddMoney(int moneyAmount){
        money+=(int)Math.Floor((float)moneyAmount*moneyMultiplier);
        StageMissionManager.Instance.CostCheck();
        UIManager.Instance.UpdateMoneyUI();
    }
    public void DecreaseMoney(int moneyAmount)
    {
        if(moneyAmount>money)return;
        money -= moneyAmount;
        StageMissionManager.Instance.CostCheck();
        UIManager.Instance.UpdateMoneyUI();
    }
    public void ChangeScene(string scene){
        SceneManager.LoadScene(scene);
    }
    public void ChangeSpeed(float multiplier){
        //burning time? or just for test
        Time.timeScale*=multiplier;
    }
    public void TriggerObj(GameObject obj){
        obj.SetActive(!obj.activeInHierarchy);
    }
    public void SetData(StageData data){
        money=data.currentStageMoney;
        dia=data.currentDia;
    }
    public void AddDataToStageData(StageData data)
    {
        data.currentStageNumber=currentBusinessStage;
        data.currentStageMoney=money;
        data.currentDia=dia;
    }
}
