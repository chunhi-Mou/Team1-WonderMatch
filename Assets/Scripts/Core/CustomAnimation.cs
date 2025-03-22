using UnityEngine;
using DG.Tweening;
public static class CustomAnimation {
    public static void ButtonPulse(Transform target, System.Action onComplete, float scaleMultiplier = 1.2f, float duration = 0.5f) {
        Vector3 originalScale = target.localScale;
        target.DOScale(originalScale * scaleMultiplier, duration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine)
            .OnComplete(() => onComplete?.Invoke());
    }
}
