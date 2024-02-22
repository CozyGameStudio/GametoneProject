using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PresetButton : MonoBehaviour
{
    public Button presetButton;
    public Image exampleImage;
    public Image ingameImage;
    public Image choiceCat;
    public TMP_Text presetName;


    public GameObject PresetWindow;
    public GameObject InteriorWindow;
    public PresetPanel presetPanel;
    public PositionPanel positionPanel;

    private bool isClickedOnce = false;

    private Preset preset;
    private PositionList posList;


    private void Start()
    {
        presetButton.onClick.AddListener(OnPresetButtonClicked);
    }

    public void SetPresetData(Preset presetData, PositionList pos)
    {
        posList = pos;
        preset = presetData;
        exampleImage.sprite = preset.interiorData.presetExampleImage;
        exampleImage.gameObject.SetActive(true);
        ingameImage.sprite = preset.interiorData.presetInGameImage;
        ingameImage.gameObject.SetActive(false);

        choiceCat.gameObject.SetActive(false);

        presetName.text = preset.interiorData.presetName;
        presetName.gameObject.SetActive(true);
    }

    private void OnPresetButtonClicked()
    {
        if (!isClickedOnce)
        {
            StartCoroutine(ChangeIngameImage());
        }
        else
        {
            presetPanel.SetPresetBool();
            presetPanel.PositionReset();
            SetPosition();
            InteriorWindow.SetActive(true);
            positionPanel.SetPosition(preset, posList);
            Debug.Log("PresetClick");
            posList.isPreset = true;
            InteriorManager.Instance.activatedPresetNum = preset.presetIndex;
            presetPanel.SetLayoutByIndex(InteriorManager.Instance.activatedPresetNum);
            InteriorManager.Instance.ComfortUpdate();
            SetExampleImage();
            PresetWindow.SetActive(false);
        }
    }

    IEnumerator ChangeIngameImage()
    {
        isClickedOnce = true;
        exampleImage.gameObject.SetActive(false);
        ingameImage.gameObject.SetActive(true);
        presetName.gameObject.SetActive(false);
        yield return new WaitForSeconds(1);
        choiceCat.gameObject.SetActive(true);
        yield return new WaitForSeconds(5);
        SetExampleImage();
    }

    private void SetExampleImage()
    {
        exampleImage.gameObject.SetActive(true);
        ingameImage.gameObject.SetActive(false);
        choiceCat.gameObject.SetActive(false);
        presetName.gameObject.SetActive(true);
        isClickedOnce = false;
    }

    private void SetPosition()
    {
        SpriteRenderer renderer;
        foreach (var obj in posList.list)
        {
            renderer = obj.positionSprites;
            renderer.gameObject.SetActive(true);
        }
    }
    private void SetLayout()
    {
        preset.layout.SetActive(true);
    }

    public void TutorialButtonClick()
    {
        if (InteriorSceneManager.Instance.currentInteriorStage == 3)
        {
            if (isClickedOnce)
            {
                if (TutorialManagerForInterior.Instance != null) TutorialManagerForInterior.Instance.PresetButtonTouch();
            }
            
        }
    }
}
