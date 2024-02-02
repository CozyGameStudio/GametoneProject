using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LoadingSceneManager : MonoBehaviour
{
    public Slider slider; 
    public static string nextScene;
    public int minLoadingTime=5;
    private void Start()
    {
        slider.maxValue=1;
        StartCoroutine(LoadGameSceneAsync());
    }
    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        if(DataSaveNLoadManager.scene.name!=nextScene)
            SceneManager.LoadScene("LoadingScene");
    }
    
    private IEnumerator LoadGameSceneAsync()
    {
        float startTime = Time.time;
        AsyncOperation asyncLoad=SceneManager.LoadSceneAsync(nextScene);
        asyncLoad.allowSceneActivation=false;
       while(!asyncLoad.isDone){
            float elapsedTime = Time.time - startTime;
            slider.value = elapsedTime / minLoadingTime;
            if (elapsedTime > 5){
                asyncLoad.allowSceneActivation = true;
            }  
            yield return null;
        }
    }
}
