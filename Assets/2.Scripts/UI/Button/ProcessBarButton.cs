using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessBarButton : MonoBehaviour
{
    public GameObject alarm;
    void Start(){
        StageMissionManager.Instance.OnRewardAbleDelegate+= IsRewardAble;
    }
    public void IsRewardAble(bool isAble){
        alarm.SetActive(isAble);
    }
    public void TutorialButtonClick()
    {
        if(BusinessGameManager.Instance==null)return;
        if (BusinessGameManager.Instance.currentBusinessStage == 1)
        {
            if (TutorialManager.Instance != null) TutorialManager.Instance.ProcessBar();
        }
    }
}
