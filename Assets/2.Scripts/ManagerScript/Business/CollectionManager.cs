using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CollectionManager : MonoBehaviour,ISystemManagerInterface
{
    public CharacterCollectionPage characterCollectionPage;
    public List<CharacterCollection> characterCollections=new List<CharacterCollection>();
    int currentPage=0;
    private static CollectionManager instance;
    public static CollectionManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CollectionManager>();
            }
            return instance;
        }
    }
    public void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    private void Start(){
        SetPage(0);
    }
    public void SetPage(int page)
    {
        currentPage = page;
        int itemsPerPage = characterCollectionPage.buttons.Count;
        int start = currentPage * itemsPerPage;
        int count = Math.Min(itemsPerPage, characterCollections.Count - start);

        if (start >= 0 && start < characterCollections.Count)
        {
            List<CharacterCollection> characterCollectionsTmp = characterCollections.GetRange(start, count);
            characterCollectionPage.SetData(characterCollectionsTmp);
        }
    }

    public void ChangePage()
    {
        int itemsPerPage = characterCollectionPage.buttons.Count;
        int totalItems = characterCollections.Count;
        int totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);

        currentPage = (currentPage + 1) % totalPages;
        SetPage(currentPage); // 페이지를 변경한 후 UI 업데이트
    }


    public void SetData(SystemData systemData){
        foreach (var characterCollection in characterCollections)
        {
            var collectionData = systemData.collectionDatas.Find(data => data.name == characterCollection.name);

            if (collectionData != null)
            {
                characterCollection.SetData(collectionData); 
            }
        }
    }
    public void AddDataToSystemData(SystemData systemData)
    {
        foreach (var characterCollection in characterCollections)
        {
            var collectionData = characterCollection.GetData();
            var existingData = systemData.collectionDatas.Find(data => data.name == collectionData.name);

            if (existingData != null)
            {
                existingData.isUnlock = collectionData.isUnlock;
            }
            else
            {
                systemData.collectionDatas.Add(collectionData);
            }
        }
    }
}
