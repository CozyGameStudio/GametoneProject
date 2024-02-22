using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class InteriorButton : MonoBehaviour
{
    public GameObject unLock;
    public TMP_Text cost;
    public Image InteriorImage;
    public Button button;

    public InteriorBuyPanel interiorBuyPanel;

    private InteriorData interior;
    private Preset preset;
    private PositionObject positionObj;

    void Start()
    {
        button.onClick.AddListener(OnInteriorButtonClick);
        interiorBuyPanel.OnBuyButtonClick += UpdateData;
    }

    public void UpdateData()
    {
        if (preset.isUnlock[interior.index] == true)
        {
            unLock.SetActive(false);
        }
        else
        {
            unLock.SetActive(true);
            cost.text = interior.interiorUnlockCost.ToString();
        }
    }

    public void SetData(InteriorData interiorData, Preset Preset, PositionObject posObj)
    {
        
        positionObj = posObj;
        interior = interiorData;
        preset = Preset;

        InteriorImage.sprite = interior.interiorImage;
        if (preset.isUnlock[interior.index] == true)
        {
            unLock.SetActive(false);
        }
        else
        {
            unLock.SetActive(true);
            cost.text = interior.interiorUnlockCost.ToString();
        }
    }

    private void OnInteriorButtonClick()
    {
        SpriteRenderer renderer = positionObj.positionSprites;

        if (preset.isUnlock[interior.index] == true)
        {
            positionObj.isPosition = true;
            positionObj.Comfort = interior.Comfort;
            positionObj.fIndex=interior.index;
            interiorBuyPanel.gameObject.SetActive(false);
            renderer.gameObject.SetActive(true);
            renderer.sprite = interior.interiorImage;
            InteriorManager.Instance.ComfortUpdate();
        }
        else
        {
            interiorBuyPanel.gameObject.SetActive(true);
            if (interior != null)
            {
                interiorBuyPanel.SetData(interior, preset, positionObj);
            }
        }
    }

    public void TutorialButtonClick()
    {
        if (InteriorSceneManager.Instance.currentInteriorStage == 3)
        {
            if (TutorialManagerForInterior.Instance != null) TutorialManagerForInterior.Instance.InteriorChoiceTouch();
        }
    }
}
