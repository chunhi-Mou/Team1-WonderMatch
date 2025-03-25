using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class StackAnimation : MonoBehaviour {
    // public void AnimateAddCard(List<CardStateManager> cardsInStack, Transform[] centerPos, int targetIndex, System.Action onComplete) {
    //     for (int i = targetIndex; i < cardsInStack.Count; i++) {
    //         cardsInStack[i].MoveCardTo(centerPos[i].position, 0.1f);
    //     }
    //     cardsInStack[targetIndex].transform
    //         .DOMove(centerPos[targetIndex].position, 0.2f)
    //         .OnComplete(() => onComplete?.Invoke());
    // }

    // public void AnimateRemoveMatch(List<CardStateManager> matchedCards, System.Action onComplete) {
    //     int completedAnimations = 0;
    //     int totalMatches = matchedCards.Count;

    //     foreach (var card in matchedCards) {
    //         card.transform.DOScale(Vector3.zero, 0.5f).OnComplete(() => {
    //             completedAnimations++;
    //             if (completedAnimations == totalMatches) {
    //                 onComplete?.Invoke();
    //             }
    //         });
    //     }
    // }
    public static void AnimateAddCard(List<CardStateManager> cardsInStack, Transform[] centerPos, int targetIndex, System.Action onComplete) {
        Sequence sequence = DOTween.Sequence();

        for (int i = targetIndex; i < cardsInStack.Count; i++) {
            sequence.Join(cardsInStack[i].transform.DOMove(centerPos[i].position, 0.5f).SetEase(Ease.OutQuad));
        }

        sequence.OnComplete(() => onComplete?.Invoke());
    }

    public static void AnimateArrangeCards(List<CardStateManager> cardsInStack, Transform[] centerPos) {
        for (int i = 0; i < cardsInStack.Count; i++) {
            CardAnimation.PlayMoveToStack(cardsInStack[i].gameObject, centerPos[i].position, () => {});
        }
    }
}
