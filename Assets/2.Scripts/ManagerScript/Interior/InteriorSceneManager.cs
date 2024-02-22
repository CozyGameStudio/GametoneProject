using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class InteriorSceneManager : MonoBehaviour
{
    private static InteriorSceneManager instance;
    public int startJelly = 100;
    public int money { get; private set; } = 0;
    public int jelly { get; private set; } = 0;
    public int comfort { get; private set; } = 0;
    public int comfortLV { get; private set; } = 1;
    public int currentInteriorStage = 3;
    public TMP_Text moneyText;

    

    public static InteriorSceneManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InteriorSceneManager>();
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



    public void AddMoney(int moneyAmount)
    {
        int currentMoney = money;
        //InteriorUIManager.Instance.SetMoneyAnimation(currentMoney, money);
    }
    public void DecreaseMoney(int moneyAmount)
    {
        if (moneyAmount > money) return;
        int currentMoney = money;
        money -= moneyAmount;
        //InteriorUIManager.Instance.SetMoneyAnimation(currentMoney, money);
    }
    public void AddDia(int diaAmount)
    {
        jelly += diaAmount;
        InteriorUIManager.Instance.UpdateDiaUI();
    }
    public void DecreaseDia(int diaAmount)
    {
        if (diaAmount > jelly) return;
        jelly -= diaAmount;
        InteriorUIManager.Instance.UpdateDiaUI();
    }
    public void ComfortUpdate(int comfortAmount)
    {
        comfort = comfortAmount;
        UpdateComfortLV();
        InteriorUIManager.Instance.UpdateComfortUI();
    }
    public void UpdateComfortLV()
    {
        if (comfort == 0)
        {
            comfortLV = 0;
        }
        if(0 < comfort && comfort <= 100)
        {
            comfortLV = 1;
        }
        else if (101 < comfort && comfort <= 200)
        {
            comfortLV = 2;
        }
        else if (201 < comfort && comfort <= 300)
        {
            comfortLV = 3;
        }
        else if (301 < comfort && comfort <= 400)
        {
            comfortLV = 4;
        }
        else if (401 < comfort && comfort <= 500)
        {
            comfortLV = 5;
        }
        InteriorUIManager.Instance.UpdateComfortLVUI();
    }
    public void SetData(SystemData systemData)
    {
        jelly=systemData.currentJelly;
        Debug.Log($"{systemData.currentJelly} / {jelly}");
        InteriorUIManager.Instance.UpdateDiaUI();
        // foreach(var preset in interiorDatas)
        // {
        //     var presetData = systemData.interiorData.preestData.Find(data => data.name.Equals(preset.interiorData.presetName));
        //     if(presetData != null)
        //     {
        //         preset.SetData(presetData);
        //     }
        // }
    }
    public void GetData(){

    }
}
