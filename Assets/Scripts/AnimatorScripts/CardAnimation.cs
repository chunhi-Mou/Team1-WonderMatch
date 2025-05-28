using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public static class CardAnimation {
    public static void PlayCardSpreadAnimation(List<Transform> cards, Transform centerPoint,
        float spreadDistance, float animationDuration, System.Action onComplete = null) {
        if (cards == null || cards.Count <= 1 || centerPoint == null) return;

        GameModeManager.instance.isUsingPowers = true;
        AudioManager.instance.PlayOneShot(SoundEffect.Shuffle);

        Vector3[] originalLocalPositions = new Vector3[cards.Count];
        Vector3[] originalLocalRotations = new Vector3[cards.Count];

        for (int i = 0; i < cards.Count; i++) {
            originalLocalPositions[i] = cards[i].localPosition;
            originalLocalRotations[i] = cards[i].localEulerAngles;
        }

        Sequence seq = DOTween.Sequence();

        int middleIndex = (cards.Count - 1) / 2;
        for (int i = 0; i < cards.Count; i++) {
            float offset = (i - (cards.Count - 1) / 2f) * spreadDistance;
            if (cards.Count % 2 == 1 && i == middleIndex)
                offset -= spreadDistance * 0.5f;

            Vector3 targetPos = originalLocalPositions[i] + new Vector3(offset, 0, 0);
            seq.Join(cards[i].DOLocalMove(targetPos, animationDuration).SetEase(Ease.OutQuad));
        }

        seq.AppendInterval(0.5f);
        seq.AppendCallback(() => AudioManager.instance.PlayOneShot(SoundEffect.ShuffleOut));

        for (int i = 0; i < cards.Count; i++) {
            Vector3 centerPos = new Vector3(centerPoint.localPosition.x, centerPoint.localPosition.y, originalLocalPositions[i].z);
            seq.Join(cards[i].DOLocalMove(centerPos, animationDuration).SetEase(Ease.InOutQuad));
        }

        seq.AppendInterval(0.5f);

        for (int i = 0; i < cards.Count; i++) {
            seq.Join(cards[i].DOLocalMove(originalLocalPositions[i], animationDuration).SetEase(Ease.OutQuad));
            seq.Join(cards[i].DOLocalRotate(originalLocalRotations[i], animationDuration).SetEase(Ease.OutQuad));
        }

        seq.AppendCallback(() => {
            GameModeManager.instance.isUsingPowers = false;
            onComplete?.Invoke();
        });
    }

    public static void StartShuffleCardsAnimations(List<Transform> cards, Transform centerPoint,
        float spreadDistance, float animationDuration, System.Action onComplete = null) {
        if (cards == null || cards.Count <= 1 || centerPoint == null)
            return;

        GameModeManager.instance.isUsingPowers = true;
        Sequence seq = DOTween.Sequence();

        Vector3[] originalLocalPositions = new Vector3[cards.Count];
        Vector3[] originalLocalRotations = new Vector3[cards.Count];

        for (int i = 0; i < cards.Count; i++) {
            originalLocalPositions[i] = cards[i].localPosition;
            originalLocalRotations[i] = cards[i].localEulerAngles;
            cards[i].rotation = Quaternion.Euler(0, 180, 0);
            seq.Append(cards[i].DORotate(new Vector3(0, 180, Random.Range(-10f, 10f)), 0f));
        }

        seq.AppendCallback(() => AudioManager.instance.PlayOneShot(SoundEffect.CardFlip));

        for (int i = 0; i < cards.Count; i++) {
            //cards[i].GetComponent<CardOverlapChecker>().NotifyTilesBelow();
            Vector3 centerPos = new Vector3(centerPoint.localPosition.x, centerPoint.localPosition.y, originalLocalPositions[i].z);
            seq.Join(cards[i].DOLocalMove(centerPos, animationDuration * 0.5f).SetEase(Ease.InOutQuad));
            seq.Join(cards[i].DOLocalRotate(new Vector3(0, 0, Random.Range(-30f, 30f)), animationDuration * 0.5f).SetEase(Ease.InOutQuad));
        }

        seq.AppendInterval(0.1f);

        float circleRadius = 0.5f;
        float angleStep = 360f / cards.Count;
        float startAngle = (cards.Count % 2 == 0) ? (angleStep / 2f) : 0f;
        for (int i = 0; i < cards.Count; i++) {
            float angle = startAngle + i * angleStep + Random.Range(-10f, 10f);
            float rad = angle * Mathf.Deg2Rad;
            Vector3 circleOffset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * circleRadius;

            seq.Join(cards[i].DOLocalMove(centerPoint.localPosition + circleOffset, 0.6f).SetEase(Ease.InOutSine));
            seq.Join(cards[i].DOLocalRotate(new Vector3(0, 180, Random.Range(-45f, 45f)), 0.6f).SetEase(Ease.InOutSine));
        }

        seq.AppendInterval(0.2f);
        seq.AppendCallback(() => AudioManager.instance.PlayOneShot(SoundEffect.Shuffle));

        float startX = -(spreadDistance * (cards.Count - 1) / 2f);
        for (int i = 0; i < cards.Count; i++) {
            float offset = (startX + i * spreadDistance) * 1.5f;
            if (Mathf.Abs(offset) < 0.1f)
                offset += 20f;

            Vector3 spreadPos = new Vector3(centerPoint.localPosition.x + offset, centerPoint.localPosition.y, originalLocalPositions[i].z);
            float t = (cards.Count == 1) ? 0.5f : (float)i / (cards.Count - 1);
            float spreadAngle = Mathf.Lerp(20, -20, t);

            seq.Join(cards[i].DOLocalMove(spreadPos, animationDuration).SetEase(Ease.OutQuad));
            seq.Join(cards[i].DOLocalRotate(new Vector3(0, 0, spreadAngle), animationDuration).SetEase(Ease.OutQuad));
        }

        seq.AppendInterval(0.5f);
        seq.AppendCallback(() => AudioManager.instance.PlayOneShot(SoundEffect.ShuffleOut));

        for (int i = 0; i < cards.Count; i++) {
            //cards[i].GetComponent<CardOverlapChecker>().NotifyTilesBelow();
            seq.Join(cards[i].DOLocalMove(originalLocalPositions[i], animationDuration).SetEase(Ease.OutQuad));
            seq.Join(cards[i].DOLocalRotate(originalLocalRotations[i], animationDuration * 0.8f).SetEase(Ease.OutBack));
        }

        seq.AppendCallback(() => {
            GameModeManager.instance.isUsingPowers = false;
            onComplete?.Invoke();
        });
    }

    public static void PlayCardShakeThenMove(Transform card, Vector3 targetPosition,
        float shakeDuration = 0.3f, float moveDuration = 0.5f, System.Action onComplete = null) {
        GameModeManager.instance.isUsingPowers = true;
        //card.GetComponent<SpriteRenderer>().sortingOrder = 1000;

        Sequence seq = DOTween.Sequence();
        seq.Append(card.DOShakePosition(shakeDuration, strength: 0.5f, vibrato: 20, randomness: 90));
        seq.Append(card.DOMove(targetPosition, moveDuration).SetEase(Ease.OutQuad));
        seq.Append(card.DOScale(Vector3.one * 0.2f, 0.2f))
            .OnComplete(() => {
                GameModeManager.instance.isUsingPowers = false;
                onComplete?.Invoke();
            });
    }
}
