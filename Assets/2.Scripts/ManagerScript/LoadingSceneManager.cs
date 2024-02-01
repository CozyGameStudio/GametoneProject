using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LoadingSceneManager : MonoBehaviour
{
    public Slider slider; 
    public static string nextScene;

    private float time=0;
    private void Start()
    {
        slider.maxValue=5;
        StartCoroutine(LoadGameSceneAsync());
    }
    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }
    
    private IEnumerator LoadGameSceneAsync()
    {
        AsyncOperation asyncLoad=SceneManager.LoadSceneAsync(nextScene);
        asyncLoad.allowSceneActivation=false;
       while(!asyncLoad.isDone){
            this.time=+Time.time;
            slider.value = time;
            if (time>5){
                asyncLoad.allowSceneActivation = true;
            }  
            yield return null;
        }
    }
}
