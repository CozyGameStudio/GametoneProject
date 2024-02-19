using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineManager : MonoBehaviour
{
    public PlayableDirector playableDirector;
    private static TimelineManager instance;
    public static TimelineManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<TimelineManager>();
            }
            return instance;
        }
    }
    // void Start()
    // {
    //     StageMissionManager.Instance.OnStageCleared+= PlayCutScene;
    // }

    public void PlayCutScene()
    {
        playableDirector.Play();
    }
    private void OnPlayableDirectorStopped(PlayableDirector aDirector)
    {
        if (playableDirector == aDirector)
        {
            
        }
    }
}
