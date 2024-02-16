using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class PositionObject 
{
    public SpriteRenderer positionSprites;
    public int Comfort;
    public bool isPosition;
}


[System.Serializable]
public class PositionList
{
    public List<PositionObject> list = new List<PositionObject>();
    public bool isPreset;
}

public class InteriorManager : MonoBehaviour
{
    private static InteriorManager instance;
    public PresetPanel presetPanel;
    public List<Preset> interiorDatas = new List<Preset>();

    public int ToTalComfort;
    
    public List<PositionList> interiorPositionObjects = new List<PositionList>();

    public static InteriorManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<InteriorManager>();
            }
            return instance;
        }
    }

    public void Awake()
    {
        if(instance != this)
        {
            Destroy(instance);
        }
    }

    private void Start()
    {
        SetPreset();
        ComfortUpdate();
    }

    public void SetPreset()
    {
        presetPanel.SetData(interiorDatas);
    }

    public void ComfortUpdate()
    {
        ToTalComfort = 0;
        PositionList list = new PositionList();

        foreach(var posObj in interiorPositionObjects)
        {
            if(posObj != null)
            {
                if(posObj.isPreset == true)
                {
                    list = posObj;
                }
            }
            else
            {
                Debug.LogError("Not found interiorPositionObjects");
            }
        }
        if(list == null)
        {
            Debug.Log("is not choice Preset");
        }
        else
        {
            foreach(var posObj in list.list)
            {
                if(posObj != null)
                {
                    if(posObj.isPosition == true)
                    {
                        ToTalComfort += posObj.Comfort;
                    }
                }
            }
        }
        InteriorSceneManager.Instance.ComfortUpdate(ToTalComfort);
    }

    public void SetData(SystemData systemData)
    {
        foreach(var preset in interiorDatas)
        {
            //var presetData = systemData.interiorData.preestData.Find(data => data.name.Equals(preset.interiorData.presetName));
            //if(presetData != null)
            //{
            //    preset.SetData(presetData);
            //}
        }
    }
    public List<PresetData> GetData()
    {
        List<PresetData> list = new List<PresetData>();
        foreach(var preset in interiorDatas)
        {
            list.Add(preset.GetData());
        }
        return list;
    }
   
}
