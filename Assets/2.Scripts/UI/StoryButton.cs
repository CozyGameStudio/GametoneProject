using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class StoryButton : MonoBehaviour
{
    public CharacterStoryPanel characterStoryPanel;
    public TMP_Text storyName;
    public Button storyButton;
    [Header("구매정보")]
    public GameObject lockPanel;
    public Image currencyImage;
    public TMP_Text storyValue;

    private Sprite characterSprite;
    
    public void SetStoryData(Sprite sprite,StoryData data)
    {
        characterSprite=sprite;
        storyButton.onClick.RemoveAllListeners();
        storyButton.onClick.AddListener(()=> characterStoryPanel.SetStory(characterSprite,data));
    }
}
