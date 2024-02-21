using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class FeverTimeUI : MonoBehaviour {
    public Image blackPanel;
    public Image fevertimeImage;

    public void PlayFeverTimeAnimation(){

        // 블랙 패널 페이드 인
        blackPanel.DOFade(1,0.2f).OnComplete(()=>{
            float targetX = 0; // 목표 x 위치. 화면의 왼쪽 끝이라고 가정
            fevertimeImage.rectTransform.DOAnchorPosX(targetX, 1.0f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                // 블랙 패널 페이드 아웃
                blackPanel.DOFade(0, 0.5f).OnComplete(() =>
                {
                    // 애니메이션 완료 후 게임 오브젝트 비활성화
                    gameObject.SetActive(false);
                });
            });
        });
        // 시작 위치를 오른쪽 끝으로 설정 (화면 크기나 레이아웃에 따라 조정 필요)
        fevertimeImage.rectTransform.anchoredPosition = new Vector2(Screen.width, fevertimeImage.rectTransform.anchoredPosition.y);
        gameObject.SetActive(false);
    }
}