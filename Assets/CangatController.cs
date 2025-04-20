using UnityEngine;
using DG.Tweening;

public class CangatController : MonoBehaviour {
    public Animator animator;
    public float animLength = 1f;
    public SlotController slotController;

    private void Start() {
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        animator.SetFloat("AnimSpeed", 1f);
        animator.Play("cangatAnim", 0, 0f);
        AudioManager.instance.Play(SoundEffect.SlotLevel);
        DOVirtual.DelayedCall(animLength, () => {
            ReverseAfterDelay();
        }).SetUpdate(true);
    }

    private void ReverseAfterDelay() {
        animator.SetFloat("AnimSpeed", -1f); 
        animator.Play("cangatAnim", 0, 1f);
        DOVirtual.DelayedCall(animLength, () => slotController.StartSlotMachineAnimation()).SetUpdate(true);
    }
    public void PlayLeverSound() {
        Debug.Log("Lever sound played");
    }

}
