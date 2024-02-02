using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class PositionBox : MonoBehaviour, IBox<ScriptableInterior>
{
    ScriptableInterior Interiors;

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
            positionText.text = "À§Ä¡ " + Interiors.position.ToString();
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
        
    }
}
