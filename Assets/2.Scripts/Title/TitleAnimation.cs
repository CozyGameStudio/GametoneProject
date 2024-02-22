using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class TitleAnimation : MonoBehaviour
{
    public Image logo;
    public Image backImage;
    public Image startButton;
    public Image titleImage;
    Sequence titleSequnce;
    void Start(){
        logo.color = new Color(logo.color.r, logo.color.g, logo.color.b, 0f);
        backImage.color = new Color(backImage.color.r, backImage.color.g, backImage.color.b, 0f);
        startButton.color = new Color(startButton.color.r, startButton.color.g, startButton.color.b, 0f);
        titleImage.color = new Color(titleImage.color.r, titleImage.color.g, titleImage.color.b, 0f);
        startButton.GetComponent<Button>().interactable=false;
        StartCoroutine(TitleAnim());
    }
    IEnumerator TitleAnim(){
        logo.DOFade(1,2);
        yield return new WaitForSeconds(4);
        titleSequnce = DOTween.Sequence()
                .Append(logo.DOFade(0, 2))
                .Append(backImage.DOFade(1, 2))
                .Append(titleImage.DOFade(1, 1))
                .Append(startButton.DOFade(1, 2))
                .OnComplete(() => startButton.GetComponent<Button>().interactable = true);
    }
}
