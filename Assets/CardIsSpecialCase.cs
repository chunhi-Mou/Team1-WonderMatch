using UnityEngine;
using UnityEngine.Events;

public class CardIsSpecialCase : MonoBehaviour {
    public UnityEvent onClick;

    private void OnMouseDown() {
        onClick?.Invoke();
        this.enabled = false;
    }
}
