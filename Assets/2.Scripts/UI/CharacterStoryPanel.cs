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
    public RectTransform textContainerBox;


    private readonly WaitForEndOfFrame _waitEndOfFrame = new WaitForEndOfFrame();
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
        UpdateSize();
    }
    public void UpdateSize()
    {
        StartCoroutine(WaitEndOfFrameAndUpdateSize());
    }

    private IEnumerator WaitEndOfFrameAndUpdateSize()
    {
        yield return _waitEndOfFrame;

        float textHeight = storyContent.preferredHeight; // Calculate the text height
        textContainerBox.sizeDelta = new Vector2(textContainerBox.sizeDelta.x, textHeight); // Adjust the container size
    }
}
