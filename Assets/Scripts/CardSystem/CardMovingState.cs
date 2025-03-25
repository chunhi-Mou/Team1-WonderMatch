using DG.Tweening;
using UnityEngine;

public class CardMovingState : CardBaseState {
    public override void EnterState(CardStateManager card) {
        card.cardsBelow = card.cardOverlapChecker.UpdateBelowCards();
    }
    public override void UpdateState(CardStateManager card) {
        card.cardInfo.spriteRenderer.sortingOrder = 1000;
        card.cardInfo.spriteRenderer.DOColor(Color.white, 0.2f);
        CardAnimation.PlayMoveToStack(card.gameObject, card.target, () => {
            card.cardInfo.cardCollider.enabled = false;
            card.SwitchState(card.inTrayState);
            GameEvents.OnCardDoneMovingInvoke();
        });
    }
}
