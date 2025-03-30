using UnityEngine;

public class ClickTrigger : MonoBehaviour {
    public Tutorial tutorial;
    public int stepToInvoke;

    private void OnMouseDown() {
        if(Tutorial.currentStep + 2 == stepToInvoke) {
            tutorial.InvokeStep(stepToInvoke-1);
        }
    }
}
