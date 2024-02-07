using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class CharacterCollectionPanel : MonoBehaviour
{
    [HideInInspector]
    public CharacterCollection currentCharacter;
    public TMP_Text characterNameText;
    public TMP_Text progressText;
    public Slider collectionSlider;
    public StoryButton[] buttons;
    public Image characterIcon;
    public Image characterStatus;
    
    public List<Sprite> currencySprite;
    //Set This to Character Collection Button
    private void Start(){
        collectionSlider.maxValue=100;
    }   
    public void SetCharacter(CharacterCollection character)
    {
        gameObject.SetActive(true);
        currentCharacter = character;
        if (currentCharacter == null)
        {
            Debug.Log("받아온 캐릭터가 없습니다......"); return;
        }
        SetUI();
        UpdateStoryList();
    }
    public void SetUI()
    {
        characterNameText.text = currentCharacter.scriptableCollection.characterNameInKorean;
        characterIcon.sprite = currentCharacter.scriptableCollection.characterIcon;
        characterStatus.sprite = currentCharacter.scriptableCollection.characterStatus;
        SetProgressBar();
    }
    public void SetProgressBar(){
        int progress = currentCharacter.GetProgressData();
        progressText.text = progress.ToString() + "%";
        collectionSlider.value = progress;
    }
    private void UpdateStoryList()
    {
        List<StoryData> srpColDataList= currentCharacter.scriptableCollection.storyDataList;
        int buttonCount = buttons.Length; // 버튼 배열의 크기
        for (int i=0;i< srpColDataList.Count;i++)
        {
            if (i >= buttonCount)
            {
                Debug.LogError($"버튼의 개수({buttonCount})가 스토리 데이터의 개수({srpColDataList.Count})보다 적습니다.");
                break; // 더 이상 처리할 수 없으므로 반복 종료
            }
            //Debug.Log(srpColDataList[i].ToString());
            int index = i;
            buttons[i].storyValue.text = srpColDataList[i].storyUnlockCost.ToString();
            buttons[i].lockPanel.GetComponent<Button>().onClick.RemoveAllListeners();
            buttons[i].lockPanel.GetComponent<Button>().onClick.AddListener(SetProgressBar);
            buttons[i].storyName.text= srpColDataList[i].storyName.ToString();
            switch (srpColDataList[i].storyCurrencyType){
                case CurrencyType.Money:
                    buttons[i].lockPanel.GetComponent<Button>().onClick.AddListener(() => currentCharacter.BuyStoryByMoney(index));
                    buttons[i].currencyImage.sprite = currencySprite[0];
                    break;
                case CurrencyType.Dia:
                    buttons[i].lockPanel.GetComponent<Button>().onClick.AddListener(() => currentCharacter.BuyStoryByDia(index));
                    buttons[i].currencyImage.sprite = currencySprite[1];
                    break;
            }
            buttons[i].lockPanel.GetComponent<Button>().onClick.AddListener(UpdateStoryList);
            buttons[i].SetStoryData(characterIcon.sprite, srpColDataList[i]);
            buttons[i].lockPanel.SetActive(!currentCharacter.isUnlock[i]);
        }
    }
}