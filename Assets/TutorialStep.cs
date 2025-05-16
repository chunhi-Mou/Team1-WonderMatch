using UnityEngine;
using UnityEngine.Events;

public class TutorialStep : MonoBehaviour {
    public UnityEvent onClick;

    private void OnMouseDown() {
        onClick?.Invoke();
        this.enabled = false;
    }
}
