using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;
using static UIButtonHandler;

public enum IdleAnimationType { None, Scale, Rotate, ScaleAndRotate }
public enum ClickAnimationType { None, Shrink, Rotate, Shake }
public enum ButtonType { Normal, Power }

[System.Serializable]
public class UIButtonHandler : MonoBehaviour {

    [Header("Button Settings")]
    [SerializeField] ButtonType buttonType = ButtonType.Normal;
    [SerializeField] SoundEffect buttonSound;
    [SerializeField] Button button;
    [SerializeField] UnityEvent onClickEvent;
    [SerializeField] bool useAnimation = true;

    [Header("Idle Animation Settings")]
    [SerializeField] IdleAnimationType idleAnimationType = IdleAnimationType.Scale;
    [SerializeField] float idleScaleFactor = 1.1f;
    [SerializeField] float idleRotationAngle = 15f;
    [SerializeField] float idleDuration = 1.5f;
    [SerializeField] Ease idleEaseType = Ease.InOutSine;
    [SerializeField] LoopType idleLoopType = LoopType.Yoyo;

    [Header("Click Animation Settings")]
    [SerializeField] ClickAnimationType clickAnimationType = ClickAnimationType.Shrink;
    [SerializeField] float clickRotationAngle = 15f;

    private Tween idleTween;

    private void Awake() {
        if (button == null) button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClicked);

        if (idleAnimationType != IdleAnimationType.None) {
            idleTween = CustomAnimation.PlayIdleAnimation(transform, idleAnimationType, idleScaleFactor, idleRotationAngle, idleDuration, idleEaseType, idleLoopType);
        }
    }

    private void OnButtonClicked() {
        var holdInfo = GetComponent<PowerButtonHoldInfo>();
        if (holdInfo != null && (holdInfo.IsHolding||holdInfo.ShowedInfo)) return;

        if (buttonType == ButtonType.Power && GameModeManager.instance.isUsingPowers) return;

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
