using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PositionPanel : MonoBehaviour
{
    public List<PositionButton> buttons;
    
    public void SetPosition(List<PositionData> positionDatas)
    {
        gameObject.SetActive(true);
        for(int i = 0; i < buttons.Count; i++)
        {
            if( i < positionDatas.Count)
            {
                buttons[i].gameObject.SetActive(true);
                buttons[i].SetData(positionDatas[i]);
            }
            else
            {
                buttons[i].gameObject.SetActive(false);
            }
        }
        if (buttons[0] != null)
        {
            buttons[0].OnPositionButtonClicked();
        }
    }




    /*public GameObject PositionPrefab;

    private List<List<ScriptableInterior>> groupInteriors; 
    private List<GameObject> positionButtons;

    // Start is called before the first frame update
    void Start()
    {
        positionButtons = new List<GameObject>();
        InitPanel();
    }

    public void InitPanel()
    {
        InteriorManager.Instance.GetPositionDatas();
        InteriorManager.Instance.ClassifyInteriorsByPosition();
        
        groupInteriors = InteriorManager.Instance.groupInteriors;

        if (positionButtons.Any())
        {
            foreach (GameObject go in positionButtons)
            {
                Destroy(go);
            }
        }

        for (int i = 0; i < groupInteriors.Count; i++)
        {
            GameObject imageObj = Instantiate(PositionPrefab);
            imageObj.transform.SetParent(transform, false);
            positionButtons.Add(imageObj);
            PositionBox positionBox = imageObj.GetComponent<PositionBox>();
            if(positionBox != null)
            {
                positionBox.InitBox(groupInteriors[i][0]);
            }
            else
            {
                Debug.LogError("Cannot find positionBox");
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/
}
