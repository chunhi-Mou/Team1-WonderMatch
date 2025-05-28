using UnityEngine;
using UnityEngine.UI;

public class UIButtonTrigger : MonoBehaviour {
    public Tutorial tutorial;
    public int stepToInvoke;
    public Button button;

    private void Start() {
        button.onClick.AddListener(() => {
            if (Tutorial.currentStep +2 == stepToInvoke) {
                tutorial.InvokeStep(stepToInvoke-1);
            }
        });
    }
}
