using DG.Tweening;
using TMPro;
using UnityEngine;

public class TimerPanel : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI timer;
    [SerializeField] private float maxTimeCount = 60f;

    public GameObject timerFill;
    public GameObject timerShow;
    public GameObject slot;

    public static float timeRemaining;
    public static float currMaxTimeCnt;
    private bool timerIsRunning = false;
    private void OnEnable() {
        timeRemaining = maxTimeCount;
        currMaxTimeCnt = maxTimeCount;
        GameEvents.OnStartTimer += StartTimer;
    }
    private void OnDisable() {
        GameEvents.OnStartTimer -= StartTimer;
    }
    private void Start() {
        timerFill.SetActive(false);
        timerShow.SetActive(false);
        slot.SetActive(true);
    }
    void Update() {
        if (timerIsRunning) {
            if (timeRemaining > 0) {
                timeRemaining -= Time.deltaTime;
                UpdateTimerDisplay(timeRemaining);
            } else {
                timeRemaining = 0;
                timerIsRunning = false;
                UpdateTimerDisplay(timeRemaining);
                GameEvents.OnLoseGameInvoke();
            }
        }
    }
    private void StartTimer() {
        timerIsRunning = true;
        timerFill.SetActive(true);
        timerShow.SetActive(true);
        slot.SetActive(false);
    }
    void UpdateTimerDisplay(float timeToDisplay) {
        timeToDisplay = Mathf.Max(0, timeToDisplay);

        int minutes = Mathf.FloorToInt(timeToDisplay / 60);
        int seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    public static void ResetTime() {
        timeRemaining = currMaxTimeCnt;
    }
}
