using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PositionPanel : MonoBehaviour
{
    public List<PositionButton> buttons;


    public void SetPosition(Preset preset, PositionList positions)
    {
        gameObject.SetActive(true);
        for(int i = 0; i < buttons.Count; i++)
        {
            if( i < preset.interiorData.positionDataList.Count)
            {
                buttons[i].gameObject.SetActive(true);
                buttons[i].SetData(preset.interiorData.positionDataList[i], preset, positions.list[i]);
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

}
