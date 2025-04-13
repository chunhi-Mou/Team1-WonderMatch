using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour {
    public GameObject shufflePower;
    public GameObject undoPower;
    public GameObject magicPower;
    public GameObject addOnePower;
    public TextMeshProUGUI tutorialTxt;
    public TextMeshProUGUI tutorialTxt2;
    public GameObject bruh;

    public static int currentStep = 0;
    private Action[] steps;
    private bool isTutorialActive = false;

    private void Start() {
        steps = new Action[] { Step1, Step2, Step3, Step4, Step5, Step6 };
        tutorialTxt.text = "";
        tutorialTxt2.text = "";
        Step1();
        DOVirtual.DelayedCall(3f, () => StartTutorial());
    }

    public void StartTutorial() {
        if (isTutorialActive) return;

        isTutorialActive = true;
        tutorialTxt.text = "SKIP";
        tutorialTxt2.text = "CLICK A CARD!";
        //RunStep();
    }

    void RunStep() {
        if (currentStep < steps.Length) {
            steps[currentStep]?.Invoke();
        }
    }

    //public void NextStep() {
    //    currentStep++;
    //    RunStep();
    //}

    public void InvokeStep(int stepIndex) {
        if (stepIndex >= 0 && stepIndex < steps.Length) {
            currentStep = stepIndex;
            RunStep();
        }
    }

    void Step1() {
        SetPowerStates(false, false, false, false);
    }

    void Step2() {
        DOVirtual.DelayedCall(1.5f, () =>
        {
            tutorialTxt2.gameObject.SetActive(false);
            SetPowerStates(false, true, false, false);
            Debug.Log("Step2: Undo power enabled");
        });
    }

    void Step3() {
        SetPowerStates(false, false, false, false);
        DOVirtual.DelayedCall(1.5f, () =>
        {
            SetPowerStates(true, false, false, false);
            Debug.Log("Step3: Shuffle power enabled");
        });
    }

    void Step4() {
        SetPowerStates(false, false, false, false);
        DOVirtual.DelayedCall(1.5f, () =>
        {
            SetPowerStates(false, false, true, false);
            Debug.Log("Step4: Magic power enabled");
        });
    }

    void Step5() {
        SetPowerStates(false, false, false, false);
        DOVirtual.DelayedCall(1.5f, () =>
        {
            SetPowerStates(false, false, false, true);
            Debug.Log("Step5: AddOne power enabled");
        });
    }


    void Step6() {
        SetPowerStates(false, false, false, false);
        bruh.gameObject.SetActive(false);
        tutorialTxt.text = "DONE!";
        DOVirtual.DelayedCall(2.5f, () => Skip());
    }
    public void Skip() {
        SceneLoader.instance.LoadScene("Map");
    }
    void SetPowerStates(bool shuffle, bool undo, bool magic, bool addOne) {
        shufflePower.SetActive(shuffle);
        undoPower.SetActive(undo);
        magicPower.SetActive(magic);
        addOnePower.SetActive(addOne);
    }
}
