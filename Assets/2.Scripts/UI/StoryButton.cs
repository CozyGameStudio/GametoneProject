using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class StoryButton : MonoBehaviour
{
    public CharacterCollectionPanel characterCollectionPanel;
    public CharacterStoryPanel characterStoryPanel;
    public TMP_Text storyName;
    public Button storyButton;
    [Header("구매정보")]
    public GameObject lockPanel;
    public Image currencyImage;
    public TMP_Text storyValue;

    private Sprite characterSprite;
    private Sprite currentSprite;
    private StoryData currentData;


    void Start()
    {
        // 리스너를 한 번만 할당합니다.
        storyButton.onClick.AddListener(OnStoryButtonClicked);
    }

    // 이 메서드를 통해 현재 스토리 데이터를 설정합니다.
    public void SetStoryData(Sprite sprite, StoryData data)
    {
        currentSprite = sprite;
        currentData = data;
    }

    // 스토리 버튼 클릭 시 호출될 메서드
    private void OnStoryButtonClicked()
    {
        if (currentData != null)
        {
            // 저장된 데이터를 사용하여 패널 설정
            characterStoryPanel.SetStory(currentSprite, currentData);
            characterCollectionPanel.gameObject.SetActive(false);
        }
    }

    public void TutorialButtonClick()
    {
        if (BusinessGameManager.Instance.currentBusinessStage == 2)
        {
            if (TutorialManagerForStageTwo.Instance != null) TutorialManagerForStageTwo.Instance.CharacterStoryButtonTouch();
        }
    }
}