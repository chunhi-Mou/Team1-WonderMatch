using UnityEngine;
using DG.Tweening;

public class CardMatchedState : CardBaseState {
    public override void EnterState(CardStateManager card) {
        //play anim xong thì Update
    }

    public override void UpdateState(CardStateManager card) {
        card.gameObject.SetActive(false);
    }
}
