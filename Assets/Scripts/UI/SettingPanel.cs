using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour {
    [Header("UI")]
    public GameObject settingsPanelBG;
    public GameObject settingsButtonObj;
    public GameObject settingsBox;
    public GameObject quitMenu;
    public Button settingButton;
    [SerializeField] SoundEffect buttonSound;

    [Header("Settings Box")]
    public Button homeButton;
    public Button replayButton;
    public Button quitButtton;


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
    public float boxScaleDuration = 0.3f;

    private Tween hourTween;
    private Tween minuteTween;

    void Start() {
        settingButton.onClick.RemoveAllListeners();
        homeButton.onClick.RemoveAllListeners();
        replayButton.onClick.RemoveAllListeners();
        quitButtton.onClick.RemoveAllListeners();

        settingButton.onClick.AddListener(SpinClockFast);
        replayButton.onClick.AddListener(RestartLevel);
        homeButton.onClick.AddListener(ShowUpQuitMenu);
        quitButtton.onClick.AddListener(QuitMidGame);

        StartIdleClock();

        settingsPanelBG.SetActive(false);
        settingsBox.SetActive(false);
        settingsButtonObj.SetActive(true);
    }

    private void OnDisable() {
        hourTween?.Kill();
        minuteTween?.Kill();
    }

    public void Resume() {
        settingsPanelBG.SetActive(false);
        settingsButtonObj.SetActive(true);
        settingsBox.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack)
            .SetUpdate(true)
            .OnComplete(() => {
            settingsBox.SetActive(false);
        });
        GameModeManager.instance.ResumeGame();
    }

    public void Pause() {
        settingsPanelBG.SetActive(true);
        settingsBox.SetActive(true);
        settingsBox.transform.localScale = Vector3.zero;
        settingsBox.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack).SetUpdate(true); ;
        GameModeManager.instance.PauseGame();
    }

    public void LoadMenu() {
        GameModeManager.instance.EnterMap();
        AudioManager.instance.ResumeAll();
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
    void ShowUpQuitMenu() {
        settingsBox.SetActive(false);
        settingsPanelBG.SetActive(false);
        quitMenu.SetActive(true);
    }
    private void StartIdleClock() {
        hourTween = hourHand.transform.DORotate(new Vector3(0, 0, -360), hourSpeed, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);

        minuteTween = minuteHand.transform.DORotate(new Vector3(0, 0, -360), minuteSpeed, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);
    }

    private void SpinClockFast() {
        minuteTween.Kill();
        //minuteTween.Kill();
        AudioManager.instance.Play(buttonSound);
        minuteHand.transform.DORotate(new Vector3(0, 0, -360), fastHourSpeed, RotateMode.FastBeyond360)
            .SetEase(Ease.OutQuad)
            .SetLoops(1, LoopType.Restart)
            .OnComplete(()=>{
                Pause();
                StartIdleClock();
            });

        //minuteHand.transform.DORotate(new Vector3(0, 0, 360), fastMinuteSpeed, RotateMode.FastBeyond360)
        //    .SetEase(Ease.OutQuad)
        //    .SetLoops(1, LoopType.Restart);
    }

}
