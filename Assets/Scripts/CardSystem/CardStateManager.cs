using System.Collections.Generic;
using UnityEngine;

public class CardStateManager : MonoBehaviour {
    public CardInfo cardInfo;
    public Vector3 target;
    public Vector3 prevPosition;
    public CardOverlapChecker cardOverlapChecker;

    public List<CardStateManager> cardsBelow = new();
    public List<CardStateManager> cardsAbove = new();

    public CardBaseState currState;
    public CardIdleState idleState = new CardIdleState();
    public CardBlockedState blockedState = new CardBlockedState();
    public CardMovingState movingState = new CardMovingState();
    public CardInTrayState inTrayState = new CardInTrayState();
    public CardMatchedState matchedState = new CardMatchedState();

    private void OnEnable() {
        GameEvents.OnCardAddedToStack += SetTarget;
    }
    private void OnDisable() {
        GameEvents.OnCardAddedToStack -= SetTarget;
    }
    void Start() {
        cardInfo = GetComponent<CardInfo>();
        cardOverlapChecker = GetComponent<CardOverlapChecker>();
        SwitchState(blockedState);
        currState.UpdateState(this);
    }
    private void OnMouseDown() {
        if (GameModeManager.instance.isPaused || currState != idleState) return;
        AudioManager.instance.Play(SoundEffect.Pop);
        GameEvents.OnCardSelectedInvoke(this);
        SwitchState(movingState);
        GameEvents.OnAddingCardInvoke(this);
    }
    public void SwitchState(CardBaseState state) {
        currState = state;
        currState.EnterState(this);
    }
    public void NotifyBelowCards() {
        foreach (var belowCard in cardsBelow) {
            belowCard.CheckStateChange();
        }
    }
    public void CheckStateChange() {
        currState.UpdateState(this);
    }
    public void SetTarget(Transform _target) {
        target = _target.position;
        if(currState == movingState) {
            currState.UpdateState(this);
        }
    }
}
