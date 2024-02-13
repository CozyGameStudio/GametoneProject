using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineManager : MonoBehaviour
{
    public PlayableDirector playableDirector;

    void Start()
    {
        StageMissionManager.Instance.OnStageCleared+= PlayCutScene;
    }

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
