using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PresetBox : MonoBehaviour, IBox<ScriptablePreset>
{
    ScriptablePreset presets;
    public Image presetImage;
    public int presetMoney;
    public TMP_Text presetNumberText;

    public Image PresetImageNotOpen;

    private Transform PresetWindow;
    private Transform InteriorWindow;
    private GameObject InteriorSystem;

    private bool isBuy = false;


    // Start is called before the first frame update
    void Start()
    {
        InteriorSystem = GameObject.FindWithTag("Interior");
    }

    public void InitBox(ScriptablePreset scriptableFromPresetManager)
    {
        presets = scriptableFromPresetManager;
        if (presetImage != null)
        {

            presetImage.sprite = presets.icon;
        }
        else
        {
            Debug.LogError("Cannot find Preset Image");
        }

        if (presetNumberText != null)
        {
            presetMoney = presets.money;
            presetNumberText.text = presetMoney.ToString();
        }
        else
        {
            Debug.LogError("Cannot find Preset Number Text");
        }
    }

    public void ButtonClick()
    {
        if(isBuy)
        {
            PresetManager.Instance.ChoicePreset(presets.number);
            PresetWindow = InteriorSystem.transform.Find("PresetWindow");
            InteriorWindow = InteriorSystem.transform.Find("InteriorWindow");
            PresetWindow.gameObject.SetActive(false);
            InteriorWindow.gameObject.SetActive(true);
        }
        else
        {
            if (InteriorSceneManager.Instance.money >= presetMoney)
            {
                PresetManager.Instance.ChoicePreset(presets.number);
                PresetWindow = InteriorSystem.transform.Find("PresetWindow");
                InteriorWindow = InteriorSystem.transform.Find("InteriorWindow");
                PresetWindow.gameObject.SetActive(false);
                InteriorWindow.gameObject.SetActive(true);
                InteriorSceneManager.Instance.AddMoney(-presetMoney);
                isBuy = true;
            }
            else
            {

            }
        }
        

    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
