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
    public StoryBuyPanel storyBuyPanel;

    public List<Sprite> currencySprite;
    //Set This to Character Collection Button
    void OnEnable(){
        if(currentCharacter!=null){
            SetUI();
            UpdateStoryList();
        }
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
    public void SetNextCharacter(){
        if (CollectionManager.Instance.characterCollections.Count == 0)
        {
            Debug.LogError("캐릭터 목록이 비어있습니다.");
            return;
        }

        // 현재 캐릭터의 인덱스를 찾음
        int currentIndex = CollectionManager.Instance.characterCollections.IndexOf(currentCharacter);
        if (currentIndex == -1)
        {
            Debug.LogError("현재 캐릭터가 목록에 없습니다.");
            return;
        }

        // 다음 캐릭터의 인덱스 계산
        int nextIndex = (currentIndex + 1) % CollectionManager.Instance.characterCollections.Count;

        // 다음 캐릭터를 현재 캐릭터로 설정
        SetCharacter(CollectionManager.Instance.characterCollections[nextIndex]);
    }
    public void SetPreviousCharacter()
    {
        if (CollectionManager.Instance.characterCollections.Count == 0)
        {
            Debug.LogError("캐릭터 목록이 비어있습니다.");
            return;
        }

        // 현재 캐릭터의 인덱스를 찾음
        int currentIndex = CollectionManager.Instance.characterCollections.IndexOf(currentCharacter);
        if (currentIndex == -1)
        {
            Debug.LogError("현재 캐릭터가 목록에 없습니다.");
            return;
        }

        // 다음 캐릭터의 인덱스 계산
        int previousIndex=(currentIndex-1).Equals(-1)? CollectionManager.Instance.characterCollections.Count-1: currentIndex - 1;

        // 다음 캐릭터를 현재 캐릭터로 설정
        SetCharacter(CollectionManager.Instance.characterCollections[previousIndex]);
    }
    public void SetUI()
    {
        if(currentCharacter!=null){
            characterNameText.text = currentCharacter.scriptableCollection.characterNameInKorean;
            characterIcon.sprite = currentCharacter.scriptableCollection.characterIcon;
            characterStatus.sprite = currentCharacter.scriptableCollection.characterStatus;
            collectionSlider.maxValue = 100;
            SetProgressBar();
        }
    }
    public void SetProgressBar()
    {
        int progress = currentCharacter.GetProgressData();
        progressText.text = progress.ToString() + "%";
        collectionSlider.value = progress;
    }
    public void UpdateStoryList()
    {
        List<StoryData> srpColDataList = currentCharacter.scriptableCollection.storyDataList;
        int buttonCount = buttons.Length; // 버튼 배열의 크기
        for (int i = 0; i < srpColDataList.Count; i++)
        {
            if (i >= buttonCount)
            {
                Debug.LogError($"버튼의 개수({buttonCount})가 스토리 데이터의 개수({srpColDataList.Count})보다 적습니다.");
                break; // 더 이상 처리할 수 없으므로 반복 종료
            }
            //Debug.Log(srpColDataList[i].ToString());
            int index = i;
            buttons[index].storyValue.text = srpColDataList[index].storyUnlockCost.ToString();
            buttons[index].lockPanel.GetComponent<Button>().onClick.RemoveAllListeners();
            buttons[index].lockPanel.GetComponent<Button>().onClick.AddListener(SetProgressBar);
            buttons[index].storyName.text = srpColDataList[index].storyName.ToString();
            Sprite tmpSprite;
            switch (srpColDataList[index].storyCurrencyType)
            {
                case CurrencyType.Money:
                    tmpSprite = currencySprite[0];
                    buttons[index].lockPanel.GetComponent<Button>().onClick.AddListener(() => storyBuyPanel.SetStoryData(currentCharacter,srpColDataList[index],index, tmpSprite));
                    buttons[index].currencyImage.sprite = tmpSprite;
                    break;
                case CurrencyType.Jelly:
                    tmpSprite = currencySprite[1];
                    buttons[index].lockPanel.GetComponent<Button>().onClick.AddListener(() => storyBuyPanel.SetStoryData(currentCharacter,srpColDataList[index],index, tmpSprite));
                    buttons[index].currencyImage.sprite = currencySprite[1];
                    break;
            }
            buttons[index].lockPanel.GetComponent<Button>().onClick.AddListener(UpdateStoryList);
            buttons[index].SetStoryData(characterIcon.sprite, srpColDataList[index]);
            buttons[index].lockPanel.SetActive(!currentCharacter.isUnlock[i]);
        }
    }
}