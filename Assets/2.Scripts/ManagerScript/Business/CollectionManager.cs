using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CollectionManager : MonoBehaviour
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
        Debug.Log("Collection Manager Data Set Start");
        foreach (var characterCollection in characterCollections)
        {
            var collectionData = systemData.collectionDatas.Find(data => data.name.Equals(characterCollection.scriptableCollection.characterName));
            if (collectionData != null)
            {
                characterCollection.SetData(collectionData);
            }
            Debug.Log(characterCollection.isUnlock.Count);
        }
    }
    public List<CollectionData> GetData()
    {
        List<CollectionData> list=new List<CollectionData>();
        foreach (var characterCollection in characterCollections)
        {
            list.Add(characterCollection.GetData());
        }
        return list;
    }
}
