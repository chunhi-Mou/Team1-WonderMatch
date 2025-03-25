using DG.Tweening;
using UnityEngine;

public class CardIdleState : CardBaseState {
    public override void EnterState(CardStateManager card) {
        card.prevPosition = card.transform.position;
        card.cardInfo.cardCollider.enabled = true;
        card.cardInfo.spriteRenderer.DOColor(Color.white, 0.2f);
    }

    public override void UpdateState(CardStateManager card) {
        if (!card.cardOverlapChecker.IsNoAboveCards()) {
            card.SwitchState(card.blockedState);
        }
    }
}
