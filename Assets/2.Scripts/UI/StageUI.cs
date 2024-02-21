using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
public class StageUI : MonoBehaviour
{
    public List<StageButton> stageButtons;
    public Slider stageProgressSlider;
    private int currentStageNumber;

    void OnEnable(){
        Debug.Log("[StageUI] OnEnable");
        currentStageNumber = DataSaveNLoadManager.Instance.businessStageNumber;
        stageProgressSlider.value= currentStageNumber;
        Debug.Log("[StageUI] "+currentStageNumber);
        if (!String.IsNullOrEmpty(currentStageNumber.ToString())){
            Debug.Log("[StageUI] Set Data");
            SetData();
        }
        if(SystemManager.Instance!=null){
            SystemManager.Instance.PlayBGMByName("localmap");
        }
    }
    private void OnDisable() {
        if (SystemManager.Instance != null)
        {
            SystemManager.Instance.PlayBGMByName(SystemManager.Instance.currentStageName);
        }
    }
    void SetData(){
        
        for (int i=0;i<stageButtons.Count;i++){
            int stageIndex = i + 1;
            if (i<currentStageNumber){
                stageButtons[i].SetStageActive(true);
                Debug.Log($"{name} index{i} set to {stageButtons[i]}");
                stageButtons[i].button.onClick.AddListener(() => CallStageChange(stageIndex));
            }
            else
            {
                stageButtons[i].SetStageActive(false);
            }
        }
    }
    void CallStageChange(int i)
    {
        DataSaveNLoadManager.Instance.SaveThings();
        //디버깅때는 비활성화
        if(i.Equals(1)|| i.Equals(2)){
            Debug.Log("Cleared Stage, But interior Stage doesnt exist");
            return;
        }
        string sceneNameToMove;
        
        if (currentStageNumber>i&& !(i.Equals(1)||i.Equals(2)))
        {
            sceneNameToMove = "InteriorStage" + i;
        }
        else{
            sceneNameToMove = "BusinessStage" + i;
            //실제 빌드 출시때는, 앞스테이지로 가는 버튼은 비활성화가 되어야함.
        }
        string currentStageName = SceneManager.GetActiveScene().name;
        Debug.Log($"{name} scene {i} set to {sceneNameToMove}");
        if (sceneNameToMove.Equals(currentStageName))return;
        LoadingSceneManager.LoadScene(sceneNameToMove);
    }
}
