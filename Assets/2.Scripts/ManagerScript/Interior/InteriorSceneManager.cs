using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class InteriorSceneManager : MonoBehaviour
{
    private static InteriorSceneManager instance;
    public List<ScriptableInterior> interiorDatas;
    //public List<ScriptableTheme> themeDatas;
    public List<ScriptablePreset> presetDatas;

    public List<Position> positionDatas;

    public int money = 0;
    public TMP_Text moneyText;

    public static InteriorSceneManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InteriorSceneManager>();
            }
            return instance;
        }
    }
    public void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void AddMoney(int add)
    {
        money += add;
        moneyText.text = money.ToString();
    }
}
