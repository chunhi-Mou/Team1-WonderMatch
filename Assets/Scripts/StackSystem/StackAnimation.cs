using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class StackAnimation : MonoBehaviour {
    public void AnimateAddCard(List<Card> cardsInStack, Transform[] centerPos, int targetIndex, System.Action onComplete) {
        for (int i = targetIndex; i < cardsInStack.Count; i++) {
            cardsInStack[i].MoveCardToStack(centerPos[i].position, 0.1f);
        }
        cardsInStack[targetIndex].transform
            .DOMove(centerPos[targetIndex].position, 0.2f)
            .OnComplete(() => onComplete?.Invoke());
    }

    public void AnimateRemoveMatch(List<Card> matchedCards, System.Action onComplete) {
        int completedAnimations = 0;
        int totalMatches = matchedCards.Count;

        foreach (var card in matchedCards) {
            card.cardVFXController.FadeOut(0.8f, ()=> {
                completedAnimations++;
                if (completedAnimations == totalMatches) {
                    onComplete?.Invoke();
                }
            });
        }
    }

    public void AnimateArrangeCards(List<Card> cardsInStack, Transform[] centerPos) {
        GameModeManager.instance.isMovingCardsInStack = true;
        for (int i = 0; i < cardsInStack.Count; i++) {
            cardsInStack[i].MoveCardTo(centerPos[i].position, 0.3f);
        }
        GameModeManager.instance.isMovingCardsInStack = false;
    }
}
