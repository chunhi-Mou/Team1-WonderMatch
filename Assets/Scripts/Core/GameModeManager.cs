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
    [SerializeField] GameObject SingleMode;
    [SerializeField] GameObject DuoMode;
    public void OnSingleModeSelected() {
        SceneManager.sceneLoaded += OnSceneLoaded; // Đăng ký sự kiện trước khi load scene
        SceneManager.LoadScene(1);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        gameMode = SingleModeManager.instance;

        if (gameMode != null) {
            SingleModeManager.instance.TurnOnObjsOfSingleMode();
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
