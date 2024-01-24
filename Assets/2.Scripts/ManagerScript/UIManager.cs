using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }
            return instance;
        }
    }
    void Awake(){
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    public TMP_Text moneyText;
    public TMP_Text moneyMultiplierText;

    public void UpdateMoneyUI(){
        moneyText.text = BusinessGameManager.Instance.money.ToString();
    }
    public void UpdateMoneyMultiplierUI()
    {
        moneyMultiplierText.text = BusinessGameManager.Instance.moneyMultiplier.ToString();
    }
}
