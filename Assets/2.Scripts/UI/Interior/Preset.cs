using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Preset : MonoBehaviour
{
    public int presetIndex;
    [SerializeField]
    public ScriptableInterior interiorData;
    public GameObject layout;
    public Dictionary<int, bool> isUnlock = new Dictionary<int, bool>();
    public void Awake()
    {
        foreach(var item in interiorData.positionDataList)
        {
            foreach (var i in item.InteriorDataList)
            {
                int key = i.index;
                isUnlock[key] = false;
            }
        }
    }

    public void BuyInteriorByJelly(int index)
    {
        int j = 0;
        InteriorData interior = new InteriorData();
        foreach( var item in interiorData.positionDataList)
        {
            foreach(var i in item.InteriorDataList)
            {
                j++;
                if(i.index == index)
                {
                    interior = i;
                    break;
                }
            }
        }
        if(InteriorSceneManager.Instance.jelly >= interior.interiorUnlockCost)
        {
            InteriorSceneManager.Instance.DecreaseDia(interior.interiorUnlockCost);
            isUnlock[interior.index] = true;
        }
    }

    public void SetData(PresetData presetData)
    {
        if(isUnlock.Count == presetData.unlocks.Count)
        {
            isUnlock = presetData.ToDictionary();
        }
    }

    public PresetData GetData()
    {
        PresetData preset = new PresetData(interiorData.name, isUnlock);
        return preset;
    }
}
