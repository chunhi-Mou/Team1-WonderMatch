using TMPro;
using UnityEngine;

public class TimerPanel : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI timer;
    [SerializeField] private float maxTimeCount = 60f;
  
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
