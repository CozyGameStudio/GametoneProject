using AssetKits.ParticleImage;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteriorUIManager : MonoBehaviour
{
    private static InteriorUIManager instance;
    public static InteriorUIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InteriorUIManager>();
            }
            return instance;
        }
    }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    public void SetData()
    {
        UpdateMoneyUI();
        UpdateDiaUI();
        UpdateComfortUI();
    }
    public TMP_Text moneyText;
    public TMP_Text jellyText;
    public TMP_Text comfortText;
    public TMP_Text comfortLVText;

    public void UpdateMoneyUI()
    {
        moneyText.text = InteriorSceneManager.Instance.money.ToString();
    }
    public void UpdateDiaUI()
    {
        jellyText.text = InteriorSceneManager.Instance.jelly.ToString();
    }
    public void UpdateComfortUI()
    {
        comfortText.text = InteriorSceneManager.Instance.comfort.ToString();
    }
    public void UpdateComfortLVUI()
    {
        comfortLVText.text = "LV " + InteriorSceneManager.Instance.comfortLV.ToString();
    }

}
