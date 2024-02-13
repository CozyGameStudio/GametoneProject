using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class SystemManager : MonoBehaviour
{
    public static SystemManager Instance { get; private set; }
    
    [Header("오디오 관련")]
    public AudioMixer masterMixer;
    public List<AudioClip> sfxs;
    private Dictionary<string, AudioClip> sfxDictionary = new Dictionary<string, AudioClip>();
    private bool isBGMOn=true;
    private AudioSource audioSource;
    
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
        foreach (var sfx in sfxs)
        {
            sfxDictionary[sfx.name] = sfx;
        }
    }
    
    private void Start()
    {
        //SetResolution();
        audioSource=GetComponent<AudioSource>();
    }
    public void PlaySFXByName(string sfxName)
    {
        if (sfxDictionary.TryGetValue(sfxName, out AudioClip clip))
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"Sound '{sfxName}' not found!");
        }
    }
    public void PlayAnimationByName(Transform targetTransform, string animationName)
    {
        Sequence sequence = DOTween.Sequence();
        switch (animationName)
        {
            case "buttonRefuse":
                sequence.Append(targetTransform.DOShakeRotation(0.5f, new Vector3(0f, 0f, 10f)))
                        .OnComplete(() => targetTransform.rotation = Quaternion.identity); // 원래의 회전 값으로 복귀
                Debug.Log("Button Refused");
                break;
            case "buttonUpgrade":
                sequence.Append(targetTransform.DOScale(1.1f, 0.1f))
                        .Append(targetTransform.DOScale(0.9f, 0.15f))
                        .Append(targetTransform.DOScale(1f, 0.1f));
                break;
            default:
                sequence.Append(targetTransform.DOScale(1.1f, 0.1f))
                        .Append(targetTransform.DOScale(0.9f, 0.15f))
                        .Append(targetTransform.DOScale(1f, 0.1f));
                break;
        }

        sequence.Play(); // 애니메이션 시퀀스 재생
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
