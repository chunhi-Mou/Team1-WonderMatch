using DG.Tweening;
using UnityEngine;

public class PlayButtonAnimator : MonoBehaviour {
    void Start() {
        CustomAnimation.ButtonPulse(transform, () => { }, (float) 1.05, (float)1.0);
    }
    private void OnDisable() {
        DOTween.Kill(transform);
    }
}
