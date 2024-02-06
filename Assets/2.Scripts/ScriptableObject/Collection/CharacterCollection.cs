using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class CollectionContent{
    string contentName;
    string content;
    public CollectionContent(string nam,string cont){
        contentName=nam;
        content=cont;
    }
}
public class CharacterCollection : MonoBehaviour
{
    public ScriptableCollection scriptableCollection;
    public bool[] isUnlock{get;private set;}
    private void Start() {
        isUnlock=new bool[scriptableCollection.storyDataList.Count];
    }
    public float GetProgressData(){
        int count=0;
        foreach(var unlock in isUnlock){
            if(unlock)count++;
        }
        return (float)(count/isUnlock.Length)*100;
    }
    public void BuyStoryByMoney(int i){
        //apply to lock panel
        if(BusinessGameManager.Instance.money>=scriptableCollection.storyDataList[i].storyUnlockCost){
            BusinessGameManager.Instance.DecreaseMoney(scriptableCollection.storyDataList[i].storyUnlockCost);
            isUnlock[i]=true;
            //연출 추가
        }
        else{
            Debug.Log("돈이 없네...?");
        }
    }
    public void BuyStoryByDia(int i)
    {
        if (i < 0 || i >= scriptableCollection.storyDataList.Count)
        {
            Debug.LogError($"인덱스 범위를 벗어났습니다: {i}");
            return;
        }
        //apply to lock panel
        if (BusinessGameManager.Instance.dia >= scriptableCollection.storyDataList[i].storyUnlockCost)
        {
            BusinessGameManager.Instance.DecreaseMoney(scriptableCollection.storyDataList[i].storyUnlockCost);
            isUnlock[i] = true;
            //연출 추가
        }
        else
        {
            Debug.Log("유료재화가 없네...?");
        }
    }
    public CollectionContent GetStoryFromData(int i){
        CollectionContent cont=new CollectionContent(scriptableCollection.storyDataList[i].storyName, scriptableCollection.storyDataList[i].storyContent);
        return cont;
    }
    public void SetData(CollectionData collectionData)
    {
        isUnlock=collectionData.isUnlock;
    }
    public CollectionData GetData(){
        CollectionData collectionData=new CollectionData(scriptableCollection.name, isUnlock);
        return collectionData;
    }
}
