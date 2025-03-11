using UnityEngine;

public class SingleModeManager : GameStateBase {
    #region Singleton - Dont destroy
    public static SingleModeManager Instance { get; private set; }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
    #endregion
    private void OnEnable() {
        this.registerEvents();
    }
    private void OnDisable() {
        this.unregisterEvents();
    }
    private void registerEvents() {
        GameEvents.OnLoseGame += LoseGame;
        GameEvents.OnWinGame += WinGame;
    }
    private void unregisterEvents() {
        GameEvents.OnLoseGame -= LoseGame;
        GameEvents.OnWinGame -= WinGame;
    }
    public void LoseGame() {
        this.TogglePause();
        //Gọi hàm trừ tim
        Debug.Log("Lose!");
    }
    public void WinGame() {
        this.TogglePause();
        //Gọi Hàm cộng coins
        Debug.Log("Win");
    }
}
