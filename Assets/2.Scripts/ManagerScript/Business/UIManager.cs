using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using AssetKits.ParticleImage;
using DG.Tweening;
public class UIManager : MonoBehaviour
{
    public delegate void StageClearedDelegate();
    public event StageClearedDelegate OnStageCleared;
    public CutScene cutScene;
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
    private void Start(){
        //StageMissionManager.Instance.OnStageCleared+=PlayCutScene;
    }
    public void SetData(){
        UpdateCurrentStageText();
        UpdateMoneyUI();
        UpdateProgress();
    }
    public TMP_Text moneyText;
    public TMP_Text diaText;
    public TMP_Text moneyMultiplierText;
    public TMP_Text currentStageText;
    public Slider slider;
    public ParticleImage coinAttraction;
    public void UpdateProgress()
    {
       slider.value=StageMissionManager.Instance.stageProgress;
    }
    public void UpdateMoneyUI(){
        moneyText.text = BusinessGameManager.Instance.money.ToString();
    }
    public void UpdateDiaUI()
    {
        diaText.text = BusinessGameManager.Instance.dia.ToString();
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

        yield return new WaitForSeconds(2);
        Debug.Log("Animation PlayedWell");

        // 파티클 애니메이션 재생 완료 후 돈 추가 로직 실행
        BusinessGameManager.Instance.AddMoney(moneyAmount);
    }
    public void SetMoneyAnimation(int currentMoney,int newMoney)
    {
        DOTween.To(() => currentMoney, x => currentMoney = x, newMoney, 1f).OnUpdate(() =>
        {
            moneyText.text = currentMoney.ToString();
        }).SetEase(Ease.OutQuad); 
    }
    public void PlayCutScene(){
        cutScene.gameObject.SetActive(true);
        StartCoroutine(cutScene.Play());
    }
}
