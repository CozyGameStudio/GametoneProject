using System;
using System.Collections;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class CutScene : MonoBehaviour
{
    public GameObject newCharacter;
    public GameObject happy;
    public delegate void CutSceneDelegate();
    public event CutSceneDelegate OnCutSceneCompleted;

    public IEnumerator Play(){
        newCharacter.SetActive(true);
        yield return new WaitForSeconds(2);
        happy.SetActive(true);
        yield return new WaitForSeconds(3);
        DataSaveNLoadManager.Instance.SceneChange();
    }
}