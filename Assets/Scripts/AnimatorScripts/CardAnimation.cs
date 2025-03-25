using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public static class CardAnimation {
    public static void PlayCardSpreadAnimation(List<Transform> cards, Transform centerPoint, float spreadDistance, float animationDuration) {
        if (cards == null || cards.Count <= 1 || centerPoint == null) return;
        AudioManager.instance.PlayOneShot(SoundEffect.Shuffle);
        Vector3[] originalPositions = new Vector3[cards.Count];

        for (int i = 0; i < cards.Count; i++) {
            originalPositions[i] = cards[i].position;
        }

        DG.Tweening.Sequence seq = DOTween.Sequence();
        int middleIndex = (cards.Count - 1) / 2;
        for (int i = 0; i < cards.Count; i++) {
            float offset = (i - (cards.Count - 1) / 2f) * spreadDistance;
            if (cards.Count % 2 == 1 && i == middleIndex) {
                offset -= spreadDistance * 0.5f;
            }
            Vector3 targetPos = originalPositions[i] + new Vector3(offset, 0, 0);
            seq.Join(cards[i].DOMove(targetPos, animationDuration).SetEase(Ease.OutQuad));
        }

        seq.AppendInterval(0.5f);
        seq.AppendCallback(() => AudioManager.instance.PlayOneShot(SoundEffect.ShuffleOut));
        for (int i = 0; i < cards.Count; i++) {
            seq.Join(cards[i].DOMove(centerPoint.position, animationDuration).SetEase(Ease.InOutQuad));
        }
        seq.AppendInterval(0.5f);
        
        for (int i = 0; i < cards.Count; i++) {
            seq.Join(cards[i].DOMove(originalPositions[i], animationDuration).SetEase(Ease.OutQuad));
        }
        seq.AppendCallback(()=>GameModeManager.instance.isUsingPowers = false);
    }
    public static void PlayCardShakeThenMove(Transform card, Vector3 targetPosition, float shakeDuration = 0.3f, float moveDuration = 0.5f, System.Action onComplete = null) {
        card.GetComponent<SpriteRenderer>().sortingOrder = 1000;
        DG.Tweening.Sequence seq = DOTween.Sequence();
        seq.Append(card.DOShakePosition(shakeDuration, strength: 0.5f, vibrato: 20, randomness: 90));
        seq.Append(card.DOMove(targetPosition, moveDuration).SetEase(Ease.OutQuad))
            .OnComplete(() => onComplete?.Invoke());
    }
    public static void PlayMoveToStack(GameObject card, Vector3 target, System.Action onComplete) {
        card.transform.DOMove(target, 0.5f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => onComplete?.Invoke());
    }
}
