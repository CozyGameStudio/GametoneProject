using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
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
    public int money=0;
    public void AddMoney(int moneyAmount){
        money+=moneyAmount;
        UIManager.Instance.UpdateMoneyUI();
    }
    public void DecreaseMoney(int moneyAmount)
    {
        if(moneyAmount>money)return;
        money -= moneyAmount;
        UIManager.Instance.UpdateMoneyUI();
    }
    public void ChangeScene(string scene){
        SceneManager.LoadScene(scene);
    }
    public void ChangeSpeed(float multiplier){
        Time.timeScale*=multiplier;
    }
}
