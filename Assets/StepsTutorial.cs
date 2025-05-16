using DG.Tweening;
using UnityEngine;

[System.Serializable]
public class StepObjs {
    public GameObject[] gameObjects;
}

public class StepsTutorial : MonoBehaviour {
    public StepObjs[] steps;
    public GameObject finalHint;
    public GameObject preventCardsFromClicked;
    public static int currentStep = 0;
    void Start() {
        preventCardsFromClicked.SetActive(false);
        ActivateStep(currentStep);
    }

    public void NextStep() {
        preventCardsFromClicked.SetActive(true);
        Debug.Log("Next");
        foreach (var obj in steps[currentStep].gameObjects) {
            obj.SetActive(false);
        }

        currentStep++;
        if (currentStep == steps.Length) {
            finalHint.SetActive(true);
            DOVirtual.DelayedCall(3f, () => {
                finalHint.SetActive(false);
                PlayerPrefs.DeleteAll();
                DOVirtual.DelayedCall(0.1f, () => SceneLoader.instance.LoadScene("Map"));
            });
        }
        if (currentStep < steps.Length) {
            ActivateStep(currentStep);
        } else {
            Debug.Log("Tutorial completed!");
        }
    }

    private void ActivateStep(int stepIndex) {
        foreach (var obj in steps[stepIndex].gameObjects) {
            obj.SetActive(true);
        }
    }
}
