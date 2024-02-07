using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCollectionButton : MonoBehaviour
{
    public CharacterCollectionPanel characterCollectionPanel;
    public Image characterImage;
    public TMP_Text characterName;
    public TMP_Text progressText;
    public Slider collectionSlider;
    public Button colButton;

    private CharacterCollection currentCharacter;
    
    void Start(){
        collectionSlider.maxValue=100;
    }
    void OnEnable(){
        if(currentCharacter!=null)SetUI();
    }
    public void SetData(CharacterCollection character)
    {
        currentCharacter=character;
        SetUI();
        colButton.onClick.RemoveAllListeners();
        colButton.onClick.AddListener(()=>characterCollectionPanel.SetCharacter(currentCharacter));
    }
    public void SetUI()
    {
        characterImage.sprite = currentCharacter.scriptableCollection.characterIcon;
        characterName.text = currentCharacter.scriptableCollection.characterNameInKorean;
        int progress = currentCharacter.GetProgressData();
        progressText.text = progress.ToString() + "%";
        collectionSlider.value = progress;
    }
}
