using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresetPanel : MonoBehaviour
{
    public List<PresetButton> button;
    public List<GameObject> PresetCommingSoonPrefab = new List<GameObject>();
    // Start is called before the first frame update

    public void SetData(List<Preset> presets)
    {
        for(int i = 0; i < 4; i++)
        {
            if(i < presets.Count)
            {
                if (button[i] != null)
                {
                    button[i].gameObject.SetActive(true);
                    button[i].SetPresetData(presets[i]);
                }
                else
                {
                    Debug.Log("ButtonError");
                }


                PresetCommingSoonPrefab[i].gameObject.SetActive(false);
                
            }
            else
            {
                button[i].gameObject.SetActive(false);
                PresetCommingSoonPrefab[i].gameObject.SetActive(true);
            }
        }
    }

    public void InitPanel()
    {


        /*PresetManager.Instance.GetPresetDatas();
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
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
