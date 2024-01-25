using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterDataList", menuName = "GametoneProject/List/CharacterDataList", order = 2)]
public class CharacterDataList : ScriptableObject
{
    public List<ScriptableCharacter> characterDataList;
}
