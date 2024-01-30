using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PresetDataList", menuName = "GametoneProject/List/PresetDataList", order = 0)]
public class PresetDataList : ScriptableObject
{
    public List<ScriptablePreset> presetDataList;

    public void AddPreset(ScriptablePreset preset)
    {
        if (preset != null && !presetDataList.Contains(preset))
        {
            presetDataList.Add(preset);
        }
    }

    public void CleanList()
    {
        presetDataList.Clear();
    }
}
