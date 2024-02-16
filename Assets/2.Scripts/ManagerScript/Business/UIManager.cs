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


    public AdUI adUI;
    public OfflineRewardUI offlineRewardUI;
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
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero, Mathf.Infinity, 1 << LayerMask.NameToLayer("InteractiveObjects"));
            if (hit.collider != null)
            {
                adUI.EnterAnimation();
            }
        }
    }

    public void SetData(){
        UpdateCurrentStageText();
        UpdateMoneyUI();
        UpdateJellyUI();
        UpdateProgress();
    }
    
    public void UpdateProgress()
    {
       slider.value=StageMissionManager.Instance.stageProgress;
    }
    public void UpdateMoneyUI(){
        moneyText.text = BusinessGameManager.Instance.money.ToString();
    }
    public void UpdateJellyUI()
    {
        jellyText.text =DataManager.Instance.jelly.ToString();
    }
    public void UpdateMoneyMultiplierUI()
    {
        moneyMultiplierText.text = BusinessGameManager.Instance.moneyMultiplier.ToString();
    }
    public void UpdateCurrentStageText(){
        currentStageText.text=BusinessGameManager.Instance.currentBusinessStage.ToString();
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
