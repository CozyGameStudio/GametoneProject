using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteriorBuyPanel : MonoBehaviour
{
    public Image interiorImage;
    public TMP_Text interiorName;
    public TMP_Text interiorComfort;
    public TMP_Text cost;

    public event Action OnBuyButtonClick;

    public Button button;

    private InteriorData interiorData;
    private Preset preset;
    private PositionObject positionObj;
    private void Start()
    {
        button.onClick.AddListener(OnInteriorBuyButtonClick);
    }

    public void SetData(InteriorData InteriorData, Preset Preset, PositionObject posObj)
    {
        interiorData = InteriorData;
        preset = Preset;
        positionObj = posObj;
        gameObject.SetActive(true);
        interiorImage.sprite = interiorData.interiorImage;
        interiorName.text = interiorData.interiorNameInKorean;
        interiorComfort.text = "+" + interiorData.Comfort.ToString();
        cost.text = interiorData.interiorUnlockCost.ToString();
    }

    public void OnInteriorBuyButtonClick()
    {
        SpriteRenderer renderer = positionObj.positionSprites;

        if (preset.isUnlock[interiorData.index] == false)
        {
            preset.BuyInteriorByJelly(interiorData.index);
        }
        if (preset.isUnlock[interiorData.index] == true)
        {
            positionObj.isPosition = true;
            positionObj.Comfort = interiorData.Comfort;
            positionObj.fIndex=interiorData.index;
            renderer.gameObject.SetActive(true);
            renderer.sprite = interiorImage.sprite;
            renderer.color = new Color(1f, 1f, 1f);

            InteriorManager.Instance.ComfortUpdate();
            this.gameObject.SetActive(false);

            OnBuyButtonClick?.Invoke();
        }

    }
}
