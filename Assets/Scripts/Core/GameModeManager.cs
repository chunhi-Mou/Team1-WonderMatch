using UnityEngine;
using UnityEngine.SceneManagement;
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
    [SerializeField] GameObject SingleMode;
    [SerializeField] GameObject DuoMode;
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
        if (isPaused) TogglePause();
        gameMode.TurnOnObjsOfMode();
        SceneManager.sceneLoaded -= OnSceneReloaded;
    }
    public void EnterMap() {
        if (isPaused) TogglePause();
        gameMode.ClearOldData();
        SceneManager.LoadScene("Map");
    }
    public void TurnOnUIAndPauseGame() {
        if (!isPaused) TogglePause();
    }
    public void TurnOffUIAndResumeGame() {
        if (isPaused) TogglePause();
    }
    public void OnSingleModeSelected() {
        SceneManager.sceneLoaded += OnSceneLoaded; // Đăng ký sự kiện trước khi load scene
        SceneManager.LoadScene(1);
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        gameMode = SingleModeManager.instance;

        if (gameMode != null) {
            SingleModeManager.instance.TurnOnObjsOfMode();
        } else {
            Debug.LogError("SingleModeManager.instance is null!");
        }

        SceneManager.sceneLoaded -= OnSceneLoaded; // Hủy đăng ký sau khi dùng
    }
    public void OnDuoModeSelected() {
        SceneManager.LoadScene(1);
        SingleMode.SetActive(false);
        DuoMode.SetActive(true);
        gameMode = DuoModeManager.instance;
    }
}
