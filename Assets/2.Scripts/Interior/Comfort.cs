using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Comfort : MonoBehaviour
{
    public List<GameObject> dialogs;
    public List<GameObject> boxs;

    private int totalComfort;
    private int comfortLv;

    public void ComfortWindow()
    {
        totalComfort = InteriorSceneManager.Instance.comfort;
        comfortLv = InteriorSceneManager.Instance.comfortLV;

        UpdateComfort();
    }

    private void UpdateComfort()
    {
        switch (comfortLv)
        {
            case 0:
                foreach (var dialog in dialogs)
                {
                    dialog.SetActive(false);
                }
                foreach (var box in boxs)
                {
                    box.SetActive(false);
                }
                break;
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
                ActiveDialog(comfortLv);
                ActiveBox(comfortLv);
                break;
        }
    }

    private void ActiveDialog(int comfortLv)
    {
        for(int i = 0; i < dialogs.Count; i++)
        {
            if(comfortLv == (i + 1))
            {
                dialogs[i].SetActive(true);
            }
            else
            {
                dialogs[i].SetActive(false);
            }
        }
    }

    private void ActiveBox(int comfortLv)
    {
        for (int i = 0; i < boxs.Count; i++)
        {
            if (comfortLv == (i + 1))
            {
                boxs[i].SetActive(true);
            }
            else
            {
                boxs[i].SetActive(false);
            }
        }
    }
}
