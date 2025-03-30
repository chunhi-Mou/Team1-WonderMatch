using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public interface IGameModeObject {
    void RegisterToGameMode();
}

public class GameModeManager : MonoBehaviour {
    #region Singleton - Dont destroy
    public static GameModeManager instance { get; private set; }

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
    #endregion
    public IGameMode gameMode;
    public bool isPaused = false;
    public bool isProcessingCard = false;
    public bool isUsingPowers = false;
    public bool isMovingCardsInStack = false;
    [SerializeField] GameObject SingleMode;
    [SerializeField] GameObject DuoMode;

    public void OnEnable() {
        GameEvents.OnCardSelected += SetCardProcessingStateTrue;
        GameEvents.OnCardDoneMoving += SetCardProcessingStateFalse;
    }
    private void Start() {
        Audio_PlayBGMusic();
    }
    private void SetCardProcessingStateTrue(Card card = null) => isProcessingCard = true;
    private void SetCardProcessingStateFalse() => isProcessingCard = false;
    public void TogglePause() {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
    }
    public void ResetGame() {
        SceneManager.sceneLoaded += OnSceneReloaded;
        gameMode.ClearOldData();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void OnSceneReloaded(Scene scene, LoadSceneMode mode) {
        StartCoroutine(DelayedSetup());
        SceneManager.sceneLoaded -= OnSceneReloaded;
    }

    private IEnumerator DelayedSetup() {
        gameMode.ClearOldData();
        yield return new WaitForEndOfFrame();
        if (isPaused) TogglePause();
        gameMode.TurnOnObjsOfMode();
    }

    public void EnterMap() {
        if (isPaused) TogglePause();
        gameMode.ClearOldData();
        SceneManager.LoadScene("Map");
    }
    public void PauseGame() {
        if (!isPaused) TogglePause();
    }
    public void ResumeGame() {
        if (isPaused) TogglePause();
    }
    public void OnSingleModeSelected() {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(1);
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        gameMode = SingleModeManager.instance;

        if (gameMode != null) {
            SingleModeManager.instance.TurnOnObjsOfMode();
        }

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    public void OnDuoModeSelected() {
        SceneManager.LoadScene(1);
        SingleMode.SetActive(false);
        DuoMode.SetActive(true);
        gameMode = DuoModeManager.instance;
    }
    #region Audio
    private void Audio_PlayBGMusic() {
        AudioManager.instance.Play(SoundEffect.BGMusic);
    }
    #endregion
}
