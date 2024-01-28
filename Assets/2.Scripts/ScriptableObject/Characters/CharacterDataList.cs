using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterDataList", menuName = "GametoneProject/List/CharacterDataList", order = 2)]
public class CharacterDataList : ScriptableObject
{
    public List<ScriptableCharacter> characterDataList;
    public void AddCharacter(ScriptableCharacter character)
    {
        if (character != null && !characterDataList.Contains(character))
        {
            characterDataList.Add(character);
        }
    }
    public void cleanList()
    {
        characterDataList.Clear();
    }
}
