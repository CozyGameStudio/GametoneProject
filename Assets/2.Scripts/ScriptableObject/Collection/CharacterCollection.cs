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
            Debug.LogError("[CharacterCollection]DividebyZeroException");
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
            SystemManager.Instance.PlaySFXByName("storySoft");
            UIManager.Instance.storyBuyPanel.gameObject.SetActive(false);
            //연출 추가
        }
        else{
            SystemManager.Instance.PlaySFXByName("buttonRefuse");
        }
    }
    public void BuyStoryByJelly(int i)
    {
        if (i < 0 || i >= scriptableCollection.storyDataList.Count)
        {
            Debug.LogError($"[CharacterCollection]인덱스 범위를 벗어났습니다: {i}");
            return;
        }
        //apply to lock panel
        if (DataManager.Instance.jelly >= scriptableCollection.storyDataList[i].storyUnlockCost)
        {
            DataManager.Instance.DecreaseJelly(scriptableCollection.storyDataList[i].storyUnlockCost);
            isUnlock[i] = true;
            //연출 추가
            SystemManager.Instance.PlaySFXByName("storyHard");
            UIManager.Instance.storyBuyPanel.gameObject.SetActive(false);
            StartCoroutine(SoundByDelay());
        }
        else
        {
            SystemManager.Instance.PlaySFXByName("buttonRefuse");
        }
    }
    public IEnumerator SoundByDelay(){
        yield return new WaitForSeconds(1f);
        SystemManager.Instance.PlaySFXByName("storySoft");
    }
    public void SetData(CollectionData collectionData)
    {
        
        if(collectionData.isUnlock.Count==isUnlock.Count)
        {
            isUnlock = collectionData.isUnlock;
        }
        Debug.Log("[CharacterCollection]Collection Data Set Called!");
        
    }
    public CollectionData GetData(){
        CollectionData collectionData=new CollectionData(scriptableCollection.name, isUnlock);
        return collectionData;
    }
}
