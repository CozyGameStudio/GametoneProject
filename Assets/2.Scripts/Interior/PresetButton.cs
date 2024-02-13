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
    public PositionPanel positionPanel;

    private bool isClickedOnce = false;

    private Preset preset;

    private void Start()
    {
        presetButton.onClick.AddListener(OnPresetButtonClicked);
    }

    public void SetPresetData(Preset presetData)
    {
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
            InteriorWindow.SetActive(true);
            positionPanel.SetPosition(preset.interiorData.positionDataList);
            Debug.Log("PresetClick");
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
}
