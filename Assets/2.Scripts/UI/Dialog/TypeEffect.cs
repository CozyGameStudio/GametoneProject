using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypeEffect : MonoBehaviour
{
    public int charPerSeconds;
    public string targetMsg;
    TMP_Text msgText;
    int index;
    float interval;

    private void Awake()
    {
        msgText = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        EffectStart();
    }

    void EffectStart()
    {
        msgText.text = "";
        interval = 1f / charPerSeconds;
        index = 0;
        Invoke("Effecting", interval);
    }

    void Effecting()
    {
        if(msgText.text == targetMsg)
        {
            EffectEnd();
            return;
        }
        msgText.text += targetMsg[index];
        index++;

        Invoke("Effecting", interval);
    }

    void EffectEnd()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
