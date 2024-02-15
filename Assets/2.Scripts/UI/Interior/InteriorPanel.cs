using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteriorPanel : MonoBehaviour
{
    public List<InteriorButton> buttons;

    public void SetInterior(List<InteriorData> interiorDatas, Preset Preset, PositionObject positionObj)
    {
        gameObject.SetActive(true);
        for(int i = 0;  i < buttons.Count; i++)
        {
            if(i < interiorDatas.Count)
            {
                buttons[i].gameObject.SetActive(true);
                buttons[i].SetData(interiorDatas[i], Preset, positionObj);
            }
            else
            {
                buttons[i].gameObject.SetActive(false);
            }
        }
    }
}
