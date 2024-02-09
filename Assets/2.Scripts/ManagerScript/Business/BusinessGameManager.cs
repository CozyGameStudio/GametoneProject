using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BusinessGameManager : MonoBehaviour,IBusinessManagerInterface
{
    private Dictionary<Character, float> characterProfits = new Dictionary<Character, float>();
    private static BusinessGameManager instance;
    public int startMoney=10;
    public int money{get;private set;}=0;
    public int dia{get;private set;}=0;
    public float moneyMultiplier=1;

    public int currentBusinessStage=1;
    public static BusinessGameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<BusinessGameManager>();
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


    void Start()
    {
        foreach (var character in FindObjectsOfType<Character>())
        {
            character.OnProfitChanged += HandleProfitChange;
            characterProfits[character] = character.currentProfitGrowthRate;
        }
        RecalculateMoneyMultiplier();
        StartAddMoney();
    }

    // 데이터 불러오기 오류 수정시 삭제
    private void StartAddMoney()
    {
        if(money == 0)
        {
            money = startMoney;
        }
        UIManager.Instance.UpdateMoneyUI();
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
        int currentMoney=money;
        money+=(int)Math.Floor((float)moneyAmount*moneyMultiplier);
        StageMissionManager.Instance.CostCheck();
        UIManager.Instance.SetMoneyAnimation(currentMoney,money);
    }
    public void DecreaseMoney(int moneyAmount)
    {
        if(moneyAmount>money)return;
        int currentMoney = money;
        money -= moneyAmount;
        StageMissionManager.Instance.CostCheck();
        UIManager.Instance.SetMoneyAnimation(currentMoney, money);
    }
    public void AddDia(int diaAmount)
    {
        dia+=diaAmount;
        UIManager.Instance.UpdateDiaUI();
    }
    public void DecreaseDia(int diaAmount)
    {
        if (diaAmount > dia) return;
        dia -= diaAmount;
        UIManager.Instance.UpdateDiaUI();
    }
    public void ChangeScene(string scene){
        SceneManager.LoadScene(scene);
    }
    public void ChangeSpeed(float multiplier){
        //burning time? or just for test
        Time.timeScale*=multiplier;
    }
    public void SetData(BusinessData data){
        money=data.currentStageMoney;
        dia=data.currentDia;
    }
    public void AddDataToBusinessData(BusinessData data)
    {
        data.currentStageNumber=currentBusinessStage;
        data.currentStageMoney=money;
        data.currentDia=dia;
    }
}
