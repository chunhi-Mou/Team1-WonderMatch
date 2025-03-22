using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public static class CardAnimation {
    public static void PlayCardSpreadAnimation(List<Transform> cards, Transform centerPoint, float spreadDistance, float animationDuration) {
        if (cards == null || cards.Count == 0 || centerPoint == null) return;

        Vector3[] originalPositions = new Vector3[cards.Count];

        for (int i = 0; i < cards.Count; i++) {
            originalPositions[i] = cards[i].position;
        }

        Sequence seq = DOTween.Sequence();
        for (int i = 0; i < cards.Count; i++) {
            Vector3 targetPos = originalPositions[i] + Vector3.right * (i - cards.Count / 2f) * spreadDistance;
            seq.Join(cards[i].DOMove(targetPos, animationDuration).SetEase(Ease.OutQuad));
        }

        seq.AppendInterval(0.5f);
        for (int i = 0; i < cards.Count; i++) {
            seq.Join(cards[i].DOMove(centerPoint.position, animationDuration).SetEase(Ease.InOutQuad));
        }

        seq.AppendInterval(0.5f);
        for (int i = 0; i < cards.Count; i++) {
            seq.Join(cards[i].DOMove(originalPositions[i], animationDuration).SetEase(Ease.OutQuad));
        }
    }
}
