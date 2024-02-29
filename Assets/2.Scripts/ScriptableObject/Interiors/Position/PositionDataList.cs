using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PositionDataList", menuName = "GametoneProject/List/PositionDataList", order = 0)]
public class PositionDataList : ScriptableObject
{
    public List<ScriptablePosition> positionDataList;

    public void AddPosition(ScriptablePosition position)
    {
        if (position != null && !positionDataList.Contains(position))
        {
            positionDataList.Add(position);
        }
    }

    public void CleanList()
    {
        positionDataList.Clear();
    }
}
