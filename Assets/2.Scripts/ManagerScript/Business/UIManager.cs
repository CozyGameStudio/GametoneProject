using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using AssetKits.ParticleImage;
using DG.Tweening;
using System;
using UnityEngine.EventSystems;
public class UIManager : MonoBehaviour
{
    public TMP_Text moneyText;
    public TMP_Text jellyText;
    public TMP_Text moneyMultiplierText;
    public TMP_Text currentStageText;
    public Slider slider;
    public ParticleImage coinAttraction;
    public ParticleImage jellyAttraction;
    public GameObject missionWindow;

    public GameObject settingPanel;
    public GameObject stageMissionPanel;
    public GameObject businessPanel;
    public GameObject collectionPanel;
    public AdUI adUI;
    public OfflineRewardUI offlineRewardUI;
    public ShopUI shopUI;
    public FeverTimeUI fevertimeUI;
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
    private void Update() {
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            bool isOverUI = false;

            // 모바일 환경에서의 UI 터치 검사
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                isOverUI = EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
            }
            // PC 환경에서의 UI 마우스 클릭 검사
            else if (Input.GetMouseButtonDown(0))
            {
                isOverUI = EventSystem.current.IsPointerOverGameObject();
            }

            if (!isOverUI)
            {
                // UI 위에 있지 않은 경우에만 처리
                Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero, Mathf.Infinity, 1 << LayerMask.NameToLayer("InteractiveObjects"));
                if (hit.collider != null)
                {
                    if (adUI.uiElement.anchoredPosition.Equals(adUI.exitPosition))
                    {
                        adUI.EnterAnimation();
                    }
                    else
                    {
                        adUI.ExitAnimation();
                    }
                }
                else
                {
                    OffAllUI();
                }
            }
        }

    }
    public void OffAllUI(){
        settingPanel.SetActive(false);
        stageMissionPanel.SetActive(false);
        businessPanel.SetActive(false);
        collectionPanel.SetActive(false);
        adUI.ExitAnimation();
        shopUI.ExitAnimation();
    }
    public void MissionWindowOff(){
        missionWindow.SetActive(false);
    }
    public void SetData(){
        UpdateCurrentStageText();
        UpdateMoneyUI();
        UpdateJellyUI();
        UpdateProgress();
    }
    public void PlayFeverTimeAnimation(){
        fevertimeUI.gameObject.SetActive(true);
        fevertimeUI.PlayFeverTimeAnimation();
    }


    public void UpdateProgress()
    {
       if(StageMissionManager.Instance!=null)slider.value=StageMissionManager.Instance.stageProgress;
    }
    public void UpdateMoneyUI(){
        if (BusinessGameManager.Instance != null) moneyText.text = BusinessGameManager.Instance.money.ToString();
    }
    public void UpdateJellyUI()
    {
        if (BusinessGameManager.Instance != null) jellyText.text =DataManager.Instance.jelly.ToString();
    }
    public void UpdateMoneyMultiplierUI()
    {
        if (BusinessGameManager.Instance != null) moneyMultiplierText.text = BusinessGameManager.Instance.moneyMultiplier.ToString();
    }
    public void UpdateCurrentStageText(){
        if (BusinessGameManager.Instance != null) currentStageText.text=BusinessGameManager.Instance.currentBusinessStage.ToString();
    }
    
    public void TriggerObj(GameObject obj)
    {
        obj.SetActive(!obj.activeInHierarchy);
    }
    public void PlaySFXByName(string sfxName){
        SystemManager.Instance.PlaySFXByName(sfxName);
    }
    public void PlayAnimationByName(Transform targetTransform, string aniName)
    {
        SystemManager.Instance.PlayAnimationByName(targetTransform, aniName);
    }
    public IEnumerator PlayCoinAttraction(Transform transform, int moneyAmount)
    {
        coinAttraction.transform.position = transform.position;
        coinAttraction.Play();
        PlaySFXByName("offlineReward");
        yield return new WaitForSeconds(2);
        Debug.Log("[UIManager]Animation PlayedWell");
        PlaySFXByName("offlineReward");
        // 파티클 애니메이션 재생 완료 후 돈 추가 로직 실행
        BusinessGameManager.Instance.AddMoney(moneyAmount);
    }
    public IEnumerator PlayJellyAttraction(Transform transform, int moneyAmount)
    {
        jellyAttraction.transform.position = transform.position;
        jellyAttraction.Play();
        PlaySFXByName("offlineReward");
        yield return new WaitForSeconds(2);
        Debug.Log("Animation PlayedWell");
        PlaySFXByName("offlineReward");
        // 파티클 애니메이션 재생 완료 후 돈 추가 로직 실행
        DataManager.Instance.AddJelly(moneyAmount);
    }
    public void SetMoneyAnimation(int currentMoney,int newMoney)
    {
        DOTween.To(() => currentMoney, x => currentMoney = x, newMoney, 1f).OnUpdate(() =>
        {
            moneyText.text = currentMoney.ToString();
        }).SetEase(Ease.OutQuad); 
    }
    public void SetJellyAnimation(int currentJelly, int newJelly)
    {
        DOTween.To(() => currentJelly, x => currentJelly = x, newJelly, 1f).OnUpdate(() =>
        {
            jellyText.text = currentJelly.ToString();
        }).SetEase(Ease.OutQuad);
    }
}
