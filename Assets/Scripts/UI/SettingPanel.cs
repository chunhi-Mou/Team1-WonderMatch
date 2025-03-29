using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour {
    [Header("UI")]
    public GameObject settingsPanelBG;
    public GameObject settingsButtonObj;
    public GameObject settingsBox;
    public Button settingButton;
    [SerializeField] SoundEffect buttonSound;

    [Header("Clock")]
    public GameObject clockCenter;
    public GameObject hourHand;
    public GameObject minuteHand;

    [Header("Animation Settings")]
    public float hourSpeed = 10f;
    public float minuteSpeed = 2f;
    public float fastHourSpeed = 100f; 
    public float fastMinuteSpeed = 20f;
    public float fastDuration = 0.5f;  

    private Tween hourTween;
    private Tween minuteTween;

    void Start() {
        StartIdleClock();

        settingButton.onClick.RemoveAllListeners();
        settingButton.onClick.AddListener(Pause);

        settingsPanelBG.SetActive(false);
        settingsBox.SetActive(false);
        settingsButtonObj.SetActive(true);

        settingButton.onClick.AddListener(SpinClockFast);
    }

    private void OnDisable() {
        hourTween?.Kill();
        minuteTween?.Kill();
    }

    public void Resume() {
        settingsPanelBG.SetActive(false);
        settingsButtonObj.SetActive(true);
        GameModeManager.instance.ResumeGame();
    }

    public void Pause() {
        settingsPanelBG.SetActive(true);
        settingsButtonObj.SetActive(false);
        GameModeManager.instance.PauseGame();
    }

    public void LoadMenu() {
        GameModeManager.instance.EnterMap();
    }

    public void RestartLevel() {
        GameModeManager.instance.ResetGame();
    }

    public void NextLevel() {
        LevelManager.UnlockNextLevel();
        LevelManager.CurrLevel++;
        RestartLevel();
    }

    public void QuitMidGame() {
        LoadMenu();
        HeartsSystem.LoseHeart();
    }

    private void StartIdleClock() {
        hourTween = hourHand.transform.DORotate(new Vector3(0, 0, 360), hourSpeed, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);

        minuteTween = minuteHand.transform.DORotate(new Vector3(0, 0, 360), minuteSpeed, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);
    }

    private void SpinClockFast() {
        hourTween.Kill();
        minuteTween.Kill();

        hourHand.transform.DORotate(new Vector3(0, 0, 360), fastHourSpeed, RotateMode.FastBeyond360)
            .SetEase(Ease.OutQuad)
            .SetLoops(3, LoopType.Restart)
            .OnComplete(StartIdleClock);

        minuteHand.transform.DORotate(new Vector3(0, 0, 360), fastMinuteSpeed, RotateMode.FastBeyond360)
            .SetEase(Ease.OutQuad)
            .SetLoops(3, LoopType.Restart);
    }

}
