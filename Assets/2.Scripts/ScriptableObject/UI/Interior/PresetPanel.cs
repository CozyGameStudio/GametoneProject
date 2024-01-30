using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresetPanel : MonoBehaviour
{
    public GameObject PresetPrefab;
    // Start is called before the first frame update
    void Start()
    {
        InitPanel();
    }

    public void InitPanel()
    {
        PresetManager.Instance.GetPresetDatas();
        PresetManager.Instance.ClassifyPositionsByPreset();
        PresetManager.Instance.ClassifyInteriorsByPreset();
        for (int i = 0; i < PresetManager.Instance.presetDatas.Count; i++)
        {
            GameObject imageObj = Instantiate(PresetPrefab);
            imageObj.transform.SetParent(transform, false);

            PresetBox presetBox = imageObj.GetComponent<PresetBox>();
            if (presetBox != null)
            {
                presetBox.InitBox(PresetManager.Instance.presetDatas[i]);
            }
            else
            {
                Debug.LogError("Cannot find PresetBox");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
