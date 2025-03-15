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
        SceneManager.LoadScene(1);
        SingleMode.SetActive(true);
        DuoMode.SetActive(false);
        gameMode = SingleModeManager.instance;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        gameMode = SingleModeManager.instance;
        SingleModeManager.instance.TurnOnObjsOfSingleMode();

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    public void OnDuoModeSelected() {
        SceneManager.LoadScene(1);
        SingleMode.SetActive(false);
        DuoMode.SetActive(true);
        gameMode = DuoModeManager.instance;
    }
}
