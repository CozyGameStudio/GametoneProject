using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageChanger : MonoBehaviour
{
    string sceneNameToMove;
    private void Start() {
        sceneNameToMove=DataSaveNLoadManager.Instance.sceneName;
    }
    public void StageLoad()
    {
        LoadingSceneManager.LoadScene(sceneNameToMove);
    }
    public void StageLoad(string name){
        LoadingSceneManager.LoadScene(name);
    }
}
