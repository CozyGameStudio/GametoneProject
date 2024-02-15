using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PositionData 
{
    public int positionNumber;
    public List<InteriorData> InteriorDataList = new List<InteriorData>();
}

[System.Serializable]
public class InteriorData
{
    public int index;
    public string interiorName;
    public string interiorNameInKorean;
    public int positionNumber;
    public int Comfort;
    public CurrencyType InteriorCurrencyType;
    public int interiorUnlockCost;
    public Sprite interiorImage;
}

[CreateAssetMenu(fileName = "Interior", menuName = "GametoneProject/Interior")]
public class ScriptableInterior : ScriptableObject
{
    public string presetName;
    public string presetNameInKorean;
    public int stageToUse;
    public List<PositionData> positionDataList = new List<PositionData>();
    public Sprite presetExampleImage;
    public Sprite presetInGameImage;
}
