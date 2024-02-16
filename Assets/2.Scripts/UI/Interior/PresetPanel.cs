using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresetPanel : MonoBehaviour
{
    public List<PresetButton> button;
    public List<GameObject> PresetCommingSoonPrefab = new List<GameObject>();

    private List<PositionList> posList;
    private List<Preset> presetDatas;
    public void SetData(List<Preset> presets)
    {
        posList = InteriorManager.Instance.interiorPositionObjects;
        presetDatas = presets;
        for(int i = 0; i < 4; i++)
        {
            if(i < presets.Count)
            {
                if (button[i] != null)
                {
                    button[i].gameObject.SetActive(true);
                    button[i].SetPresetData(presets[i], posList[i]);
                }
                else
                {
                    Debug.Log("ButtonError");
                }


                PresetCommingSoonPrefab[i].gameObject.SetActive(false);
                
            }
            else
            {
                button[i].gameObject.SetActive(false);
                PresetCommingSoonPrefab[i].gameObject.SetActive(true);
            }
        }
    }

    public void SetPresetBool()
    {
        foreach (var interiorPositionObj in InteriorManager.Instance.interiorPositionObjects)
        {
            interiorPositionObj.isPreset = false;
        }
    }

    public void LayoutReset()
    {
        foreach (var preset in presetDatas)
        {
            //preset.layout.SetActive(false);
        }
    }

    public void PositionReset()
    {
        SpriteRenderer renderer;
        foreach(var posObj in posList)
        {
            foreach(var obj in posObj.list)
            {
                renderer = obj.positionSprites;
                renderer.gameObject.SetActive(false);
            }
        }
    }
}
