using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class SystemManager : Singleton<SystemManager>
{
    public AudioMixer masterMixer;
    private bool isBGMOn=true;
    void Awake(){
        Input.multiTouchEnabled = false;

#if UNITY_ANDROID
        Application.targetFrameRate = 60;
#else
        QualitySettings.vSyncCount=1;
#endif
    }
    public void TriggerAudio_BGM(){
        isBGMOn=!isBGMOn;
        if(!isBGMOn)
            masterMixer.SetFloat("BGM", -80f);
        else
            masterMixer.SetFloat("BGM",0f);
        Debug.Log("Audio set");
    }
}
