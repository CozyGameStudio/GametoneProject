using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    public Image bgmButton;
    public Image sfxButton;
    public Sprite on;
    public Sprite off;
    void OnEnable(){
        SetBGMButton();
        SetSFXButton();
    }
    public void TriggerAudio_BGM(){
        if(SystemManager.Instance!=null){
            SystemManager.Instance.TriggerAudio_BGM();
            SetBGMButton();
        }
    }
    public void TriggerAudio_SFX()
    {
        if (SystemManager.Instance != null) {
            SystemManager.Instance.TriggerAudio_SFX();
            SetSFXButton();
            }
    }
    public void SetBGMButton(){
        if (SystemManager.Instance.isBGMOn)
        {
            bgmButton.sprite = on;
        }
        else
        {
            bgmButton.sprite = off;
        }
    }
    public void SetSFXButton()
    {
        if (SystemManager.Instance.isSFXOn)
        {
            sfxButton.sprite = on;
        }
        else
        {
            sfxButton.sprite = off;
        }
    }
    
}
