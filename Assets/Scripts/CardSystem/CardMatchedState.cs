using UnityEngine;
using DG.Tweening;

public class CardMatchedState : CardBaseState {
    public override void EnterState(CardStateManager card) {
        //play anim xong th� Update
    }

    public override void UpdateState(CardStateManager card) {
        card.gameObject.SetActive(false);
    }
}
