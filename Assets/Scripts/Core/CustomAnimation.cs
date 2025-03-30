using UnityEngine;
using DG.Tweening;

public static class CustomAnimation {
    public static Tween PlayClickAnimation(Transform target, ClickAnimationType type, float rotationAngle, System.Action onComplete = null) {
        switch (type) {
            case ClickAnimationType.Shrink:
                return target.DOScale(0.9f, 0.1f).SetEase(Ease.OutQuad)
                    .OnComplete(() => target.DOScale(1f, 0.1f).SetEase(Ease.OutQuad).OnComplete(() => onComplete?.Invoke()));
            case ClickAnimationType.Rotate:
                return target.DORotate(new Vector3(0, 0, rotationAngle), 0.1f, RotateMode.LocalAxisAdd)
                    .SetEase(Ease.OutQuad)
                    .OnComplete(() => target.DORotate(Vector3.zero, 0.1f, RotateMode.LocalAxisAdd).SetEase(Ease.OutQuad).OnComplete(() => onComplete?.Invoke()));
            case ClickAnimationType.Shake:
                return target.DOShakePosition(0.2f, 10, 10).OnComplete(() => onComplete?.Invoke());
            default:
                onComplete?.Invoke();
                return null;
        }
    }

    public static Tween PlayIdleAnimation(Transform target, IdleAnimationType type, float scaleFactor, float rotationAngle, float duration, Ease easeType, LoopType loopType) {
        switch (type) {
            case IdleAnimationType.Scale:
                return target.DOScale(scaleFactor, duration)
                    .SetLoops(-1, loopType)
                    .SetEase(easeType);
            case IdleAnimationType.Rotate:
                return target.DORotate(new Vector3(0, 0, rotationAngle), duration)
                    .SetLoops(-1, loopType)
                    .SetEase(easeType);
            case IdleAnimationType.ScaleAndRotate:
                return DOTween.Sequence()
                    .Append(target.DOScale(scaleFactor, duration).SetEase(easeType))
                    .Join(target.DORotate(new Vector3(0, 0, rotationAngle), duration).SetEase(easeType))
                    .SetLoops(-1, loopType);
            default:
                return null;
        }
    }
    public static Tween PlayExitAnimation(Transform transform, System.Action onComplete = null) {
        return DOTween.Sequence()
            .Append(transform.DOScale(1.2f, 0.1f)) 
            .Append(transform.DOScale(1f, 0.1f)) 
            .SetLoops(3) 
            .OnComplete(() => {
               transform.DOScale(0.5f, 0.2f)
                   .SetEase(Ease.InQuad)
                   .OnComplete(() => {
                       onComplete?.Invoke();
                   });
           });
    }

}