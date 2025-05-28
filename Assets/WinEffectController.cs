using UnityEngine;
using DG.Tweening;

public class WinEffectController : MonoBehaviour {
    public ParticleSystem[] winParticles;

    void Start() {
        if (winParticles == null || winParticles.Length == 0) {
            Debug.LogWarning("Chưa gán ParticleSystem vào script!");
        }
        foreach (var effect in winParticles) {
            effect.Stop();
            effect.Clear();
            effect.Stop();
        }
    }

    public void PlayEffect() {
        for (int i = 0; i < winParticles.Length; i++) {
            var effect = winParticles[i];
            if (effect != null) {
                float delay = Random.Range(0f, 0.7f);
                DOVirtual.DelayedCall(delay, () => {
                    effect.Play();
                }).SetUpdate(true);
            }
        }

    }

    public void StopEffect() {
        if (winParticles != null) {
            foreach (var effect in winParticles) {
                if (effect != null) effect.Stop();
            }
        }
    }

    public void RestartEffect() {
        if (winParticles != null) {
            foreach (var effect in winParticles) {
                if (effect != null) {
                    effect.Stop();
                    effect.Clear();
                    effect.Play();
                }
            }
        }
    }
}
