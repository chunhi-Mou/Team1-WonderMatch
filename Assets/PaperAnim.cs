using UnityEngine;
using DG.Tweening;

public class PaperAnim : MonoBehaviour {
    public RectTransform PaperUp;
    public RectTransform PaperDown;
    public float moveDuration = 0.03f;

    private RectTransform rectTransform;

    private void OnEnable() {
        rectTransform = GetComponent<RectTransform>();
        DOTween.Kill(rectTransform);
        rectTransform.anchoredPosition = PaperUp.anchoredPosition;
        rectTransform.DOAnchorPos(PaperDown.anchoredPosition, moveDuration)
                     .SetEase(Ease.Unset)
                     .SetUpdate(true);
    }
}
