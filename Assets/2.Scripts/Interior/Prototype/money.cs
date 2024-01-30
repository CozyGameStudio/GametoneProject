using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class money : MonoBehaviour
{
    public TextMeshProUGUI moneyText;
    // Start is called before the first frame update
    void Start()
    {
        moneyText.text = "2000";
    }

    public void buyDecorate(int n)
    {
        int mNum = int.Parse(moneyText.text);
        mNum -= n;
        moneyText.text = mNum.ToString();
    }
}
