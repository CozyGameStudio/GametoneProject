using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FeverTimeUI : MonoBehaviour {
    public Image blackPanel;
    public Image fevertimeImage;
    public Vector2 enterPosition;
    public Vector2 exitPosition;

    public void PlayFeverTimeAnimation(){
        Sequence seq=DOTween.Sequence()
        .Append(blackPanel.DOFade(.8f, 0.2f))
        .Append(fevertimeImage.rectTransform.DOAnchorPos(exitPosition, 4f).SetEase(Ease.OutQuad))
        .Append(blackPanel.DOFade(0, 0.5f))
        .OnComplete(()=> gameObject.SetActive(false));
        seq.Play();
        fevertimeImage.rectTransform.anchoredPosition = enterPosition;
    }
}