using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

[System.Serializable]
public class UIButtonHandler : MonoBehaviour {
    public enum IdleAnimationType { None, Scale, Rotate, ScaleAndRotate }
    public enum ClickAnimationType { None, Shrink, Rotate, Shake }

    [Header("Button Settings")]
    [SerializeField] SoundEffect buttonSound;
    [SerializeField] private Button button;
    [SerializeField] private UnityEvent onClickEvent;
    [SerializeField] private bool useAnimation = true;

    [Header("Idle Animation Settings")]
    [SerializeField] private IdleAnimationType idleAnimationType = IdleAnimationType.Scale;
    [SerializeField] private float idleScaleFactor = 1.1f;
    [SerializeField] private float idleRotationAngle = 15f;
    [SerializeField] private float idleDuration = 1.5f;
    [SerializeField] private Ease idleEaseType = Ease.InOutSine;
    [SerializeField] private LoopType idleLoopType = LoopType.Yoyo;

    [Header("Click Animation Settings")]
    [SerializeField] private ClickAnimationType clickAnimationType = ClickAnimationType.Shrink;
    [SerializeField] private float clickRotationAngle = 15f;

    private Tween idleTween;

    private void Awake() {
        if (button == null) button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClicked);

        if (idleAnimationType != IdleAnimationType.None) {
            idleTween = CustomAnimation.PlayIdleAnimation(transform, idleAnimationType, idleScaleFactor, idleRotationAngle, idleDuration, idleEaseType, idleLoopType);
        }
    }

    private void OnButtonClicked() {
        if (idleTween != null) {
            idleTween.Kill();
            transform.localScale = Vector3.one;
            transform.rotation = Quaternion.identity;
        }

        if (useAnimation) {
            AudioManager.instance.Play(buttonSound);
            CustomAnimation.PlayClickAnimation(transform, clickAnimationType, clickRotationAngle, () => onClickEvent?.Invoke());
        } else {
            onClickEvent?.Invoke();
        }
    }
}
