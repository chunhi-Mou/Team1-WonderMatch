using UnityEngine;
using DG.Tweening;

public class CardMatchedState : CardBaseState {
    public override void EnterState(CardStateManager card) {
        CardAnimation.PlayCardsMatched(card.gameObject, () => { UpdateState(card); });
    }

    public override void UpdateState(CardStateManager card) {
        card.gameObject.SetActive(false);
        GameEvents.OnMatchTilesDoneInvoke();
    }
}
