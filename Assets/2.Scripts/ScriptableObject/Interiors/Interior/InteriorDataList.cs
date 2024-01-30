using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InteriorDataList", menuName = "GametoneProject/List/InteriorDataList", order = 0)]
public class InteriorDataList : ScriptableObject
{
    public List<ScriptableInterior> interiorDataList;

    public void AddInterior(ScriptableInterior interior)
    {
        if(interior != null && !interiorDataList.Contains(interior))
        {
            interiorDataList.Add(interior);
        }
    }

    public void CleanList()
    {
        interiorDataList.Clear();
    }
}
