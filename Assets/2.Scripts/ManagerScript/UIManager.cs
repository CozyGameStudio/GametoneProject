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
    void Start(){
        UpdateCurrentStageText();
    }
    public TMP_Text moneyText;
    public TMP_Text moneyMultiplierText;
    public TMP_Text currentStageText;

    public void UpdateMoneyUI(){
        moneyText.text = BusinessGameManager.Instance.money.ToString();
    }
    public void UpdateMoneyMultiplierUI()
    {
        moneyMultiplierText.text = BusinessGameManager.Instance.moneyMultiplier.ToString();
    }
    public void UpdateCurrentStageText(){
        currentStageText.text=BusinessGameManager.Instance.currentBusinessStage.ToString();
    }
}
