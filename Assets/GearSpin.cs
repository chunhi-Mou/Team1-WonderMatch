using UnityEngine;
using DG.Tweening;

public class GearSpin : MonoBehaviour {
    [SerializeField] private float speed = 100f;
    private void Start() {
        RotateGear();
    }
    private void RotateGear() {
        transform.DORotate(new Vector3(0, 0, -360), 1f / (speed / 360), RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear);
    }
}
