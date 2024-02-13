using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CharacterHiringTutorial : TutorialBase
{
    public Image dialogueBox;
    public TMP_Text dialogueText;
    public GameObject darkFilter;
    public GameObject uiDarkFilter;
    public GameObject businessButtonFilter;
    public GameObject businessBoxFilter;
    [Header("나레이션 데이터")]
    public List<string> dialogueDataList;
    public List<Transform> dialogueBoxPositionDataList;
    public override void Enter()
    {
        throw new System.NotImplementedException();
    }
    public override void Execute(TutorialManagerForStage2 manager)
    {
        throw new System.NotImplementedException();
    }
    public override void Exit()
    {
        throw new System.NotImplementedException();
    }
}
