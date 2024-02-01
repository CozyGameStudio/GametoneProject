using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TitleAnimation : MonoBehaviour
{
    public Image logo;
    public Image backImage;
    public Image startButton;
    Sequence titleSequnce;
    private bool isAnimationDone=false;
    void Start(){
        logo.color = new Color(logo.color.r, logo.color.g, logo.color.b, 0f);
        backImage.color = new Color(backImage.color.r, backImage.color.g, backImage.color.b, 0f);
        startButton.color = new Color(startButton.color.r, startButton.color.g, startButton.color.b, 0f);
        StartCoroutine(TitleAnim());
    }
    IEnumerator TitleAnim(){
        logo.DOFade(1,2);
        yield return new WaitForSeconds(4);
        titleSequnce = DOTween.Sequence()
                .Append(logo.DOFade(0, 2))
                .Append(backImage.DOFade(1, 2)).Append(startButton.DOFade(1, 2)).OnComplete(() => isAnimationDone = true);
    }
}
