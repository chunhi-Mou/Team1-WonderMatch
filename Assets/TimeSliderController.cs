using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TimeSliderController : MonoBehaviour {
    public Slider timeSlider;
    public RectTransform[] lightOn;
    public RectTransform handle;
    private bool[] turnedOff;
    private bool[] isBlinking;
    private Tween[] blinkTweens;

    private void OnEnable() {
        GameEvents.OnWinGame += SaveStarCount;
    }
    private void OnDisable() {
        GameEvents.OnWinGame -= SaveStarCount;
    }

    void Start() {
        timeSlider.maxValue = TimerPanel.timeRemaining;
        timeSlider.value = TimerPanel.timeRemaining;

        handle = timeSlider.handleRect;
        turnedOff = new bool[lightOn.Length];
        isBlinking = new bool[lightOn.Length];
        blinkTweens = new Tween[lightOn.Length];
    }

    void Update() {
        timeSlider.value = TimerPanel.timeRemaining;
        for (int i = 0; i < lightOn.Length; i++) {
            if (!turnedOff[i] && !isBlinking[i]) {
                float handleX = handle.position.x;
                float lightX = lightOn[i].position.x;

                if (handleX <= lightX) {
                    int index = i;
                    isBlinking[index] = true;
                    turnedOff[index] = true;

                    if (blinkTweens[index] != null) blinkTweens[index].Kill();

                    blinkTweens[index] = DOVirtual.DelayedCall(0f, () => {
                        BlinkLight(index);
                    }, false);
                }
            }
        }
    }


    void BlinkLight(int index) {
        int blinkTimes = 3;
        float interval = 0.2f;
        Sequence seq = DOTween.Sequence().SetUpdate(true).SetAutoKill(true);

        for (int j = 0; j < blinkTimes; j++) {
            seq.AppendCallback(() => lightOn[index].gameObject.SetActive(false));
            seq.AppendInterval(interval);
            seq.AppendCallback(() => lightOn[index].gameObject.SetActive(true));
            seq.AppendInterval(interval);
        }

        seq.AppendCallback(() => {
            lightOn[index].gameObject.SetActive(false);
        });

        blinkTweens[index] = seq;
    }

    void SaveStarCount() {
        int starCount = 0;
        for (int i = 0; i < turnedOff.Length; i++) {
            if (!turnedOff[i]) starCount++;
        }
        StarsSystems.stars = starCount;
        StarsSystems.instance.SaveStarsData();
    }
}

