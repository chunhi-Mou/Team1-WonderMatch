using UnityEngine;

public abstract class CardBaseState {
    public abstract void EnterState(CardStateManager cardStateManager);
    public abstract void UpdateState(CardStateManager cardStateManager);
}
