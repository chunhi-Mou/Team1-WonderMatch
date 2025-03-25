using UnityEngine;

public class CardInTrayState : CardBaseState {
    public override void EnterState(CardStateManager card) {
        card.NotifyBelowCards();
        card.cardInfo.spriteRenderer.sortingOrder = 0;
    }

    public override void UpdateState(CardStateManager card) {
        card.SwitchState(card.matchedState);
    }
}
