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

    private void Start()
    {
        positionButton.onClick.AddListener(OnPositionButtonClicked);
    }
    public void SetData(PositionData positionData)
    {
        position = positionData;
        positionNumber.text = "위치 " + position.positionNumber.ToString();
    }

    public void OnPositionButtonClicked()
    {
        interiorPanel.SetInterior(position.InteriorDataList);
    }
   /* ScriptableInterior Interiors;

    private GameObject InteriorSystem;
    private InteriorPanel interiorPanel;
    public TMP_Text positionText;

    // Start is called before the first frame update
    void Start()
    {
        InteriorSystem = GameObject.FindWithTag("Interior");
    }

    public void InitBox(ScriptableInterior scriptableFormInteriorManager)
    {
        Interiors = scriptableFormInteriorManager;
        if(positionText != null)
        {
            positionText.text = "위치 " + Interiors.position.ToString();
        }
        else
        {
            Debug.LogError("Cannot find Interior position Text");
        }

    }

    public void ButtonClick()
    {
        InteriorPanel[] panels = InteriorSystem.GetComponentsInChildren<InteriorPanel>(true);
        interiorPanel = panels.FirstOrDefault(p => p.name == "InteriorPanel");
        if (interiorPanel != null)
        {
            interiorPanel.InitPanel(Interiors.position);
        }
        else
        {
            Debug.LogError("InteriorPanel component not found in InteriorSystem");
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }*/
}
