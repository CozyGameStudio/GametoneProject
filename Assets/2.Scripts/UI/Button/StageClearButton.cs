
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
public class StageClearButton : MonoBehaviour
{

    public Image text;
    public Image heart;
    public Image button;


    private void OnEnable() {
        Sequence seq=DOTween.Sequence()
        .Append(text.DOFade(1,1))
        .Append(heart.DOFade(1, 1))
        .Append(button.DOFade(1, 1));
    }
    void Update()
    {
        // 화면에 터치가 있는지 확인
        if (Input.touchCount > 0)
        {
            // 첫 번째 터치를 가져옴
            Touch touch = Input.GetTouch(0);

            // 터치가 시작되었는지 확인
            if (touch.phase == TouchPhase.Began)
            {
                // 터치 위치를 월드 좌표로 변환
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                touchPosition.z = 0; // Z축 조정

                // Raycast를 통해 터치된 객체 확인
                RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);
                if (hit.collider != null && hit.collider.gameObject == button.gameObject)
                {
                    DataSaveNLoadManager.Instance.SceneChange();
                }
            }
        }
    }
}