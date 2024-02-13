using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteriorManager : MonoBehaviour
{
    private static InteriorManager instance;
    public PresetPanel presetPanel;
    public List<Preset> interiorDatas = new List<Preset>();

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
    }

    public void SetPreset()
    {
        presetPanel.SetData(interiorDatas);
    }

    public void SetData()
    {

    }

    public List<ScriptableInterior> GetData() 
    {
        List<ScriptableInterior> list = new List<ScriptableInterior>();
        return list;
    } 
}
