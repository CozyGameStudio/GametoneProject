using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour
{
    public GameObject loadingScreen;
    public GameObject gameStart;

    private float time=0;
    public void LoadSceneWithLoadingScreen(string sceneName){
        StartCoroutine(LoadGameSceneAsync(sceneName));
        // if(sceneName.Contains("Interior")){
        //     DataSaveNLoadManager.Instance.SaveGameObjectsFromBusiness();
        // }
    }
    private IEnumerator LoadGameSceneAsync(string sceneName)
    {
        loadingScreen.SetActive(true);
        AsyncOperation asyncLoad=SceneManager.LoadSceneAsync("sceneName");
        asyncLoad.allowSceneActivation=false;
       while(!asyncLoad.isDone){
            this.time=+Time.time;
            if(time>5)
                asyncLoad.allowSceneActivation=true;
            yield return null;
        }
    }
}
