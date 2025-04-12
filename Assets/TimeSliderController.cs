using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TimeSliderController : MonoBehaviour {
    public Slider timeSlider;
    public RectTransform[] lightOn;
    public RectTransform handle;
    private bool[] turnedOff;
    private bool[] isBlinking;

    void Start() {
        timeSlider.maxValue = TimerPanel.timeRemaining;
        timeSlider.value = TimerPanel.timeRemaining;

        handle = timeSlider.handleRect;
        turnedOff = new bool[lightOn.Length];
        isBlinking = new bool[lightOn.Length];

    }

    void Update() {
        timeSlider.value = TimerPanel.timeRemaining;
        for (int i = 0; i < lightOn.Length; i++) {
            if (!turnedOff[i] && !isBlinking[i]) {
                float handleX = handle.position.x;
                float lightX = lightOn[i].position.x;

                if (handleX <= lightX) {
                    isBlinking[i] = true;
                    int index = i;

                    Sequence seq = DOTween.Sequence();
                    int blinkTimes = 3;
                    float interval = 0.2f;

                    for (int j = 0; j < blinkTimes; j++) {
                        seq.AppendCallback(() => lightOn[index].gameObject.SetActive(false));
                        seq.AppendInterval(interval);
                        seq.AppendCallback(() => lightOn[index].gameObject.SetActive(true));
                        seq.AppendInterval(interval);
                    }

                    seq.AppendCallback(() => {
                        lightOn[index].gameObject.SetActive(false);
                        turnedOff[index] = true;
                    });
                }
            }
        }

    }
}
