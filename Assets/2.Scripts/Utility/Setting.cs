using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting : MonoBehaviour
{
    public void TriggerAudio_BGM(){

        SystemManager.Instance?.TriggerAudio_BGM();
    }
}
