using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CurrencyType{
    Money,
    Jelly
}
[System.Serializable]
public class StoryData
{
    public string storyName;
    public CurrencyType storyCurrencyType;
    public int storyUnlockCost;
    public string storyContent;

    public override string ToString(){
        return storyName+" "+ storyCurrencyType+" "+ storyUnlockCost+" "+ storyContent;
    }
}
[CreateAssetMenu(fileName = "Collection", menuName = "GametoneProject/Collection", order = 4)]
public class ScriptableCollection : ScriptableObject
{
    public string characterName;
    public string characterNameInKorean;
    public List<StoryData> storyDataList= new List<StoryData>();
    public Sprite characterIcon;
    public Sprite characterStatus;
}
