using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CharacterStoryPanel : MonoBehaviour
{
    public Image characterImage;
    public TMP_Text storyName;
    public TMP_Text storyContent;

    public void SetStory(Sprite sprite,StoryData storyData)
    {
        gameObject.SetActive(true);
        characterImage.sprite= sprite;
        if (storyData == null)
        {
            Debug.Log("받아온 스토리가 없습니다......"); return;
        }
        storyName.text = storyData.storyName;
        storyContent.text=storyData.storyContent;
    }
}
