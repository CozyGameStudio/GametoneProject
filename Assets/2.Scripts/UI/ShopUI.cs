using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;
public class ShopUI : MonoBehaviour
{
    public Vector2 enterPosition;
    public Vector2 exitPosition;
    public float animationDuration = 0.5f;

    public GameObject adLoadingPanel;
    private RectTransform uiElement;
    [Header("일일보상버튼")]
    public List<ShopRewardButton> dailyRewardButtons;
    [Header("화폐전환")]
    public Button coinToJellyButton;
    public Button jellyToCoinButton;
    public TMP_Text coinToJellyRemaining;
    public TMP_Text jellyToCoinRemaining;

    [Header("광고보상")]
    public Button coinRewardButton;
    public Button jellyRewardButton;
    public TMP_Text coinRewardRemaining;
    public TMP_Text jellyRewardRemaining;
    private Dictionary<RewardType, TMP_Text> rewardCountTexts;
    private Dictionary<RewardType, Button> rewardButtons;
    public void EnterAnimation()
    {
        uiElement.DOAnchorPos(enterPosition, animationDuration).SetEase(Ease.OutBack);
    }

    public void ExitAnimation()
    {
        uiElement.DOAnchorPos(exitPosition, animationDuration)
            .SetEase(Ease.InBack);
    }
    public void InitADUI(){
        uiElement = GetComponent<RectTransform>();
        uiElement.anchoredPosition = exitPosition;
        rewardCountTexts = new Dictionary<RewardType, TMP_Text>
        {
            { RewardType.CoinBonus, coinRewardRemaining },
            { RewardType.JellyBonus, jellyRewardRemaining }
        };
        rewardButtons = new Dictionary<RewardType, Button>
        {
            { RewardType.CoinBonus,coinRewardButton},
            { RewardType.JellyBonus, jellyRewardButton}
        };
        foreach (RewardType rewardType in Enum.GetValues(typeof(RewardType)))
        {
            SetRemainTextForAd(rewardType);
        }

    }
    public void InitShopUI(int currentDayCount){
        //일일 보상 체크
        for(int i=0;i< ShopManager.Instance.isRewardReceived.Length; i++){
            if(i< currentDayCount){
                dailyRewardButtons[i].activeImage.SetActive(true);
                if (!ShopManager.Instance.isRewardReceived[i]){
                    dailyRewardButtons[i].button.interactable=true;
                    dailyRewardButtons[i].receivedRewardText.gameObject.SetActive(false);
                }
                else{
                    dailyRewardButtons[i].button.interactable = false;
                    dailyRewardButtons[i].receivedRewardText.gameObject.SetActive(true);
                }
            }
            else{
                dailyRewardButtons[i].activeImage.SetActive(false);
                dailyRewardButtons[i].button.interactable = false;
                dailyRewardButtons[i].receivedRewardText.gameObject.SetActive(false);
            }
        }
        if(ShopManager.Instance.isRewardReceived[6]){
            //특별보상은 마지막 날 보상을 받으면 활성화
            dailyRewardButtons[7].activeImage.SetActive(true);
            dailyRewardButtons[7].button.interactable = true;
        }
        SetRemainTextForChange();
    }
    public void SetRemainTextForChange()
    {
        coinToJellyRemaining.text=ShopManager.Instance.coinToJellyCount.ToString();
        jellyToCoinRemaining.text = ShopManager.Instance.jellyToCoinCount.ToString();
    }
    void SetRemainTextForAd(RewardType rewardType)
    {
        if (rewardCountTexts.TryGetValue(rewardType, out TMP_Text text))
        {
            int remainingCount = AdMobManager.Instance.GetRemainingAdsCount(rewardType);
            int maxCount = AdMobManager.Instance.GetMaxAdsCount();
            if (remainingCount.Equals(0))
            {
                text.text = "0";
            }
            else
            {
                text.text = $"{remainingCount}";
            }
        }
    }

} 