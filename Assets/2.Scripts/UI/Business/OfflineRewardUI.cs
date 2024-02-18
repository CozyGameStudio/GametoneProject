using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
public class OfflineRewardUI : MonoBehaviour
{
    private RectTransform uiElement;
    public Vector2 enterPosition;
    public Vector2 exitPosition;
    public TMP_Text earningText;
    public float animationDuration = 0.5f;
    public GameObject adLoadingPanel;
    private int earnings;
    void Start(){
        uiElement=GetComponent<RectTransform>();
    }
    // UI 활성화 시 보상 금액 설정
    public void SetEarnings(int newEarnings)
    {
        earnings = newEarnings;
        earningText.text=earnings.ToString();
    }
    public void GetNormalReward(){
        if(BusinessGameManager.Instance!=null)BusinessGameManager.Instance.AddMoney(earnings);
        SystemManager.Instance.PlaySFXByName("offlineReward");
        DataSaveNLoadManager.Instance.SaveThings();
        ExitAnimation();
    }
    public void GetAdReward(){
        adLoadingPanel.SetActive(true);
        if (AdMobManager.Instance != null) AdMobManager.Instance.LoadRewardedAdForOffline(earnings);
        SystemManager.Instance.PlaySFXByName("offlineReward");
        ExitAnimation();
    }
    public IEnumerator EnterAnimation()
    {
        Debug.Log($"[OfflinerewardUI] play Animation Enter");
        yield return new WaitForSeconds(0.3f);
        uiElement.DOAnchorPos(enterPosition, animationDuration).SetEase(Ease.OutBack);
    }

    public void ExitAnimation()
    {
        uiElement.DOAnchorPos(exitPosition, animationDuration)
            .SetEase(Ease.InBack);
    }
}
