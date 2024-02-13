using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManagerForStage2 : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> tutorialDialogues;
    [SerializeField]
    private string nextTutorialName="";

    private int currentIndex=0;
    private void Start() {
        
    }
    private void StartTutorial(){

    }
    public void SetnextTutorial(){
        tutorialDialogues[currentIndex].SetActive(false);
        currentIndex++;
        tutorialDialogues[currentIndex].SetActive(true);
    }
    
    
}
