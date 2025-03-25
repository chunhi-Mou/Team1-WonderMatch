using UnityEngine;

public abstract class StackBaseState 
{
    public abstract void EnterState(StackStateManager stackStateManager);
    public abstract void UpdateState(StackStateManager stackStateManager);
}
