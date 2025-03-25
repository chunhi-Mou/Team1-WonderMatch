using UnityEngine;
using DG.Tweening;

public class CardBlockedState : CardBaseState {
    public override void EnterState(CardStateManager card) {
        card.cardInfo.spriteRenderer.DOColor(new Color(123f / 255f, 122f / 255f, 122f / 255f, 1f), 0.2f);
    }

    public override void UpdateState(CardStateManager card) {
        if (card.cardOverlapChecker.IsNoAboveCards()) {
            card.SwitchState(card.idleState);
        }
    }
}
