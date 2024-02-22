using UnityEngine.UI;
using UnityEngine;
using TMPro;
public class StageButton : MonoBehaviour
{
    public Button button;
    public Image buttonImage;
    public Sprite activeSprite;
    public Sprite inActiveSprite;
    public TMP_Text clearText;
    public void SetStageActive(bool isActive){
        buttonImage.sprite=isActive?activeSprite:inActiveSprite;
        button.interactable=isActive;
    }


    public void TutorialButtonClick()
    {
        if (BusinessGameManager.Instance.currentBusinessStage == 4)
        {
            if (TutorialManagerForStageFour.Instance != null) TutorialManagerForStageFour.Instance.InteriorEnterButtonTouch();
        }
    }
}
