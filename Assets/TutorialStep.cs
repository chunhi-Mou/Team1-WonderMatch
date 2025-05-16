using UnityEngine;
using UnityEngine.Events;

public class TutorialStep : MonoBehaviour {
    public UnityEvent onClick;

    private void OnMouseDown() {
        if (GameModeManager.instance.isUsingPowers) return;
        onClick?.Invoke();
        this.enabled = false;
    }
}
