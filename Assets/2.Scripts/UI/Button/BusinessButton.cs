using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusinessButton : MonoBehaviour
{
    public GameObject alarm;
    void Start(){
        DataManager.Instance.OnCurrencyChangeDelegate+= SetAlarmBubble;
        BusinessGameManager.Instance.OnCurrencyChangeDelegate += SetAlarmBubble;
    }
    
    public void SetAlarmBubble(){
        if(alarm!=null)alarm.SetActive(DataManager.Instance.CheckAbleUpgrade());
    }
    public void TutorialButtonClick()
    {
        if (TutorialManager.Instance != null) TutorialManager.Instance.BusinessButtonTouch();
        Debug.Log("Click");
    }

}
