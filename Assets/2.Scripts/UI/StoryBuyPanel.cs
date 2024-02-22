using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoryBuyPanel : MonoBehaviour{
    public TMP_Text storyName;
    public TMP_Text currency;
    public Image currencyImage;
    public Button buyButton;
    public Button uiOffButton;

    void Start(){
        uiOffButton.onClick.AddListener(() => gameObject.SetActive(false));
        uiOffButton.onClick.AddListener(()=> UIManager.Instance.PlaySFXByName("buttonNormal"));
    }
    public void SetStoryData(CharacterCollection currentCharacter,StoryData storyData,int i,Sprite sprite){
        gameObject.SetActive(true);
        UIManager.Instance.PlaySFXByName("buttonNormal");
        storyName.text=storyData.storyName;
        currency.text=storyData.storyUnlockCost.ToString();
        currencyImage.sprite = sprite;
        buyButton.onClick.RemoveAllListeners();
        switch (storyData.storyCurrencyType){
            case CurrencyType.Money:
                buyButton.onClick.AddListener(()=> currentCharacter.BuyStoryByMoney(i));
            break;
            case CurrencyType.Jelly:
                buyButton.onClick.AddListener(() => currentCharacter.BuyStoryByJelly(i));
                break;
        }
    }
}