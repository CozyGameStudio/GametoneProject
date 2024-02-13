using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
[Serializable]
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
    public List<bool> isUnlock;
    public void Awake(){
        isUnlock = new List<bool>(new bool[scriptableCollection.storyDataList.Count]);
    }
    public int GetProgressData(){
        int count=0;
        foreach(var unlock in isUnlock){
            if(unlock)count++;
        }
        if (isUnlock.Count == 0) {
            Debug.LogError("DividebyZeroException");
            return 0;
        }
        int pro= count * 100 / isUnlock.Count;
        Debug.Log(pro);
        return pro;
    }
    public void BuyStoryByMoney(int i){
        //apply to lock panel
        if(BusinessGameManager.Instance.money>=scriptableCollection.storyDataList[i].storyUnlockCost){
            BusinessGameManager.Instance.DecreaseMoney(scriptableCollection.storyDataList[i].storyUnlockCost);
            isUnlock[i]=true;
            UIManager.Instance.PlaySFXByName("pay");
            //연출 추가
        }
        else{
            Debug.Log("돈이 없네...?");
            UIManager.Instance.PlaySFXByName("buttonRefuse");
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
            UIManager.Instance.PlaySFXByName("pay");
        }
        else
        {
            Debug.Log("유료재화가 없네...?");
            UIManager.Instance.PlaySFXByName("buttonRefuse");
        }
    }
    public void SetData(CollectionData collectionData)
    {
        
        if(collectionData.isUnlock.Count==isUnlock.Count)
        {
            isUnlock = collectionData.isUnlock;
        }
        Debug.Log("Collection Data Set Called!");
        
    }
    public CollectionData GetData(){
        CollectionData collectionData=new CollectionData(scriptableCollection.name, isUnlock);
        return collectionData;
    }
}
