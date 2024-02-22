using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCollectionButton : MonoBehaviour
{
    public CharacterCollectionPage characterCollectionPage;
    public CharacterCollectionPanel characterCollectionPanel;
    public Image characterImage;
    public TMP_Text characterName;
    public TMP_Text progressText;
    public Slider collectionSlider;
    public Button colButton;

    private CharacterCollection currentCharacter;

    void Start()
    {
       
        colButton.onClick.AddListener(OnCollectionButtonClick);
    }

    void OnEnable()
    {
        UpdateUI();
    }

    public void SetData(CharacterCollection character)
    {
        currentCharacter = character;
        UpdateUI();
    }

    private void OnCollectionButtonClick()
    {
        if (currentCharacter != null)
        {
            characterCollectionPanel.SetCharacter(currentCharacter);
            PlaySFXByName("buttonNormal");
            characterCollectionPage.gameObject.SetActive(false);
        }
    }

    private void UpdateUI()
    {
        if (currentCharacter != null)
        {
            characterImage.sprite = currentCharacter.scriptableCollection.characterIcon;
            characterName.text = currentCharacter.scriptableCollection.characterNameInKorean;
            int progress = currentCharacter.GetProgressData();
            progressText.text = progress.ToString() + "%";
            collectionSlider.maxValue = 100;
            collectionSlider.value = progress;
        }
    }
    private void PlaySFXByName(string sfxName)
    {
        SystemManager.Instance.PlaySFXByName(sfxName);
    }
    private void PlayAnimationByName(Transform targetTransform, string aniName)
    {
        SystemManager.Instance.PlayAnimationByName(targetTransform, aniName);
    }

    public void TutorialButtonClick()
    {
        if (BusinessGameManager.Instance.currentBusinessStage == 2)
        {
            if (TutorialManagerForStageTwo.Instance != null) TutorialManagerForStageTwo.Instance.CharacterCollectionChoiceTouch();
        }
    }
}
