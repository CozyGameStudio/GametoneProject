using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCollectionPage : MonoBehaviour
{
    public List<CharacterCollectionButton> buttons;
    public void SetData(List<CharacterCollection> characterCollection)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if (i < characterCollection.Count)
            {
                buttons[i].gameObject.SetActive(true); 
                buttons[i].SetData(characterCollection[i]);
            }
            else
            {
                buttons[i].gameObject.SetActive(false); 
            }
        }
    }
}
