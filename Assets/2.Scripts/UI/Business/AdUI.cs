using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
public class AdUI : MonoBehaviour
{
    public Button cooktimeReward;
    public Button profitReward;
    public Button speedReward;

    
    public Vector2 enterPosition; 
    public Vector2 exitPosition; 
    public float animationDuration = 0.5f; 

    public GameObject adLoadingPanel;
    public RectTransform uiElement{get;private set;}
    private Dictionary<RewardType, TMP_Text> rewardTexts;
    private Dictionary<RewardType, Button> rewardButtons;
    private float waitTime=6f;
    // Start is called before the first frame update
    public void InitUI()
    {
        uiElement = GetComponent<RectTransform>();
        uiElement.anchoredPosition = exitPosition;
        // 버튼과 텍스트 매핑 초기화
        rewardTexts = new Dictionary<RewardType, TMP_Text>
        {
            { RewardType.Cooktime, cooktimeReward.GetComponentInChildren<TMP_Text>() },
            { RewardType.Profit, profitReward.GetComponentInChildren<TMP_Text>() },
            { RewardType.Speed, speedReward.GetComponentInChildren<TMP_Text>() }
        };

        rewardButtons = new Dictionary<RewardType, Button>
        {
            { RewardType.Cooktime, cooktimeReward },
            { RewardType.Profit, profitReward },
            { RewardType.Speed, speedReward }
        };
        OrderManager.Instance.OnRewardTimeCheckDelegate += (timeLeft) => UpdateRewardStatus(RewardType.Cooktime, timeLeft);
        CustomerManager.Instance.OnRewardTimeCheckDelegate += (timeLeft) => UpdateRewardStatus(RewardType.Profit, timeLeft);
        DataManager.Instance.OnRewardTimeCheckDelegate += (timeLeft) => UpdateRewardStatus(RewardType.Speed, timeLeft);
        foreach (RewardType rewardType in Enum.GetValues(typeof(RewardType)))
        {
            SetRemainText(rewardType);
        }
        
    }

    public void EnterAnimation()
    {
        uiElement.DOAnchorPos(enterPosition, animationDuration).SetEase(Ease.OutBack);
    }

    public void ExitAnimation()
    {
        uiElement.DOAnchorPos(exitPosition, animationDuration)
            .SetEase(Ease.InBack); 
    }
    public void UpdateRewardStatus(RewardType rewardType, float timeLeft)
    {
        if (timeLeft.Equals(0))
        {
            StartCoroutine(RewardCooltime(rewardType, waitTime));
        }
        else
        {
            if (rewardButtons.TryGetValue(rewardType, out Button button))
            {
                button.interactable = false;
            }
            if (rewardTexts.TryGetValue(rewardType, out TMP_Text text))
            {
                TimeSpan timeSpan = TimeSpan.FromSeconds(timeLeft);
                text.text = $"활성화 시간\n{timeSpan:mm\\:ss}";
            }
        }
    }

    IEnumerator RewardCooltime(RewardType rewardType, float time)
    {
        float timeLeft = time;
        while (timeLeft > 0)
        {
            if (rewardTexts.TryGetValue(rewardType, out TMP_Text textWait))
            {
                TimeSpan timeSpan = TimeSpan.FromSeconds(timeLeft);
                textWait.text = $"대기 시간\n{timeSpan:mm\\:ss}";
            }
            yield return new WaitForSeconds(1f);
            timeLeft -= 1f;
        }
        SetRemainText(rewardType);
        if (rewardButtons.TryGetValue(rewardType, out Button button))
        {
            button.interactable = true;
        }
        
    }
    void SetRemainText(RewardType rewardType)
    {
        if (rewardTexts.TryGetValue(rewardType, out TMP_Text text))
        {
            int remainingCount = AdMobManager.Instance.GetRemainingAdsCount(rewardType);
            int maxCount = AdMobManager.Instance.GetMaxAdsCount();
            if (remainingCount<=0)
            {
                text.text = "주어진 광고를\n모두 봤어요!";
                if (rewardButtons.TryGetValue(rewardType, out Button button))
                {
                    button.interactable = false;
                }
            }
            else{
                text.text = $"광고보기\n{remainingCount} / {maxCount}";
            }
        }
    }
}
