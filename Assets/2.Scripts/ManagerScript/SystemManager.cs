using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SystemManager : MonoBehaviour
{
    public static SystemManager Instance { get; private set; }
    
    [Header("오디오 관련")]
    public AudioMixer masterMixer;
    private List<AudioClip> bgms=new List<AudioClip>();
    private List<AudioClip> sfxs = new List<AudioClip>();
    private Dictionary<string, AudioClip> sfxDictionary = new Dictionary<string, AudioClip>();
    private bool isBGMOn=true;
    private AudioSource audioSource;
    private AudioClip currentAudioClip;
    public string currentStageName{get;private set;}
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
        //리소스 동적 추가
        foreach (var bgm in Resources.LoadAll<AudioClip>("BGM"))
        {
            bgms.Add(bgm);
        }
        foreach (var sfx in Resources.LoadAll<AudioClip>("SFX"))
        {
            sfxs.Add(sfx);
        }
        foreach (var sfx in sfxs)
        {
            sfxDictionary[sfx.name] = sfx;
        }
        
    }
    
    private void Start()
    {
        audioSource=GetComponent<AudioSource>();
        currentStageName=SceneManager.GetActiveScene().name;
        //스테이지 이름을 받아서 해당 String을 포함하고 있는 BGM 을 찾는다
        if(currentStageName.Contains("Business")){
            PlayBGMByName(currentStageName);
        }
        else if(currentStageName.Contains("Interior"))
        {
            PlayBGMByName("interior");
        }else if (currentStageName.Contains("Title"))
        {
            PlayBGMByName("intro");
        } 
    }
    public void PlayBGMByName(string bgmName){
        currentAudioClip = bgms.Find(data => bgmName.Contains(data.name));
        audioSource.clip = currentAudioClip;
        audioSource.Play();
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

    
    public void TriggerAudio_BGM(){
        isBGMOn=!isBGMOn;
        if(!isBGMOn)
            masterMixer.SetFloat("BGM", -80f);
        else
            masterMixer.SetFloat("BGM",0f);
        Debug.Log("Audio set");
    }
    public void SetData(SystemData systemData){
        isBGMOn=systemData.systemSettingData.isBGMOn;
    }
    public SystemSettingData GetData(){
        SystemSettingData systemSettingData=new SystemSettingData(isBGMOn);
        return systemSettingData;
    }
    
}
