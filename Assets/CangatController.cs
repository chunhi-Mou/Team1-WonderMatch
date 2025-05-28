using UnityEngine;
using DG.Tweening;

public class CangatController : MonoBehaviour {
    public Animator animator;
    public float animLength = 0.25f;
    public SlotController slotController;

    private void Start() {
        DOVirtual.DelayedCall(0.5f, () => {
            animator.updateMode = AnimatorUpdateMode.UnscaledTime;
            animator.SetFloat("AnimSpeed", 2f);
            animator.Play("cangatAnim", 0, 0f);
            AudioManager.instance.Play(SoundEffect.SlotLevel);
            DOVirtual.DelayedCall(animLength, () => {
                ReverseAfterDelay();
            }).SetUpdate(true);

        });
    }

    private void ReverseAfterDelay() {
        animator.SetFloat("AnimSpeed", -2f); 
        animator.Play("cangatAnim", 0, 1f);
        DOVirtual.DelayedCall(animLength, () => slotController.StartSlotMachineAnimation()).SetUpdate(true);
    }
    public void PlayLeverSound() {
       
    }

}
