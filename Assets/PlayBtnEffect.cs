using UnityEngine;
using DG.Tweening;

public class PlayBtnEffect : MonoBehaviour {
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    void Start() {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null) {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        StartBlinkEffect();
    }

    void StartBlinkEffect() {
        canvasGroup.DOFade(0.5f, 0.8f).SetLoops(-1, LoopType.Yoyo);
    }
}
