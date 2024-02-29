
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
    public void CallSceneChange(){
        DataSaveNLoadManager.Instance.SceneChange();
    }
    
}