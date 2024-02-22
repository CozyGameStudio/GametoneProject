using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class PositionButton : MonoBehaviour//, IBox<ScriptableInterior>
{
    public Button positionButton;
    public TMP_Text positionNumber;

    public InteriorPanel interiorPanel;

    private PositionData position;
    private Preset preset;
    private PositionObject positionObj;
    private void Start()
    {
        positionButton.onClick.AddListener(OnPositionButtonClicked);
    }
    public void SetData(PositionData positionData, Preset Preset, PositionObject posObj)
    {
        positionObj = posObj;
        position = positionData;
        preset = Preset;
        positionNumber.text = "À§Ä¡ " + position.positionNumber.ToString();
    }

    public void OnPositionButtonClicked()
    {
        interiorPanel.SetInterior(position.InteriorDataList, preset, positionObj);
    }

    public void TutorialButtonClick()
    {
        if (InteriorSceneManager.Instance.currentInteriorStage == 3)
        {
            if (TutorialManagerForInterior.Instance != null) TutorialManagerForInterior.Instance.InteriorPositionTouch();
        }
    }
}
