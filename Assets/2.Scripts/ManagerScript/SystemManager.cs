using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class SystemManager : MonoBehaviour, ISystemManagerInterface
{
    public static SystemManager Instance { get; private set; }
    public AudioMixer masterMixer;
    private bool isBGMOn=true;
    void Awake(){
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Ensure it persists across scene loads
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // Ensure only one instance exists
        }
#if UNITY_ANDROID
        Application.targetFrameRate = 60;
#else
        QualitySettings.vSyncCount=1;
#endif
    }
    private void Start()
    {
        SetResolution();
    }

    public void SetResolution()
    {
        int setWidth = 1920; 
        int setHeight = 1080; 

        int deviceWidth = Screen.width;
        int deviceHeight = Screen.height; 

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true); 

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight)
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); 
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); 
        }
        else
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); 
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); 
        }
    }
    public void TriggerAudio_BGM(){
        isBGMOn=!isBGMOn;
        if(!isBGMOn)
            masterMixer.SetFloat("BGM", -80f);
        else
            masterMixer.SetFloat("BGM",0f);
        Debug.Log("Audio set");
    }
    public void SetData(SystemData systemData){
        
    }
    public void AddDataToSystemData(SystemData systemData){

    }
}
