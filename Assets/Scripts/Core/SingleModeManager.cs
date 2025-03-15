using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SingleModeManager : MonoBehaviour, IGameMode {
    #region Singleton - Dont destroy
    public static SingleModeManager instance { get; private set; }

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

    #region IGameMode
    private bool isPaused = false;
    public bool IsPaused => isPaused;
    public void TogglePause() {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
    }
    public void ResetGame() {
        Time.timeScale = 1;
        isPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    #endregion
    private void OnEnable() {
        RegisterEvents();
    }
    private void OnDisable() {
        UnregisterEvents();
    }
    private void RegisterEvents() {
        GameEvents.OnLoseGame += LoseGame;
        GameEvents.OnWinGame += WinGame;
    }
    private void UnregisterEvents() {
        GameEvents.OnLoseGame -= LoseGame;
        GameEvents.OnWinGame -= WinGame;
    }
    public void LoseGame() {
        TogglePause();
        Debug.Log("Lose!");
    }
    public void WinGame() {
        TogglePause();
        CoinsManager.Instance.AddCoinsOnWin(); //20 coins
        Debug.Log("Win");
    }
    [SerializeField] GameObject PowerUpUI;
    [SerializeField] List<Player> players = new List<Player>();
    public void SetPowerUpUI(GameObject obj) {
        PowerUpUI = obj;
    }
    public void RegisterPlayer(Player player) {
        if (!players.Contains(player)) {
            players.Add(player);
        }
    }
    public void TurnOnObjsOfSingleMode() {
        if (PowerUpUI != null) PowerUpUI.SetActive(true);
        foreach (var player in players) {
            if (player.CompareTag("PlayerA")) { //Nhi: Player A is default
                player.gameObject.SetActive(true);
            }
        }
    }
    public void TurnOffObjsOfSingleMode() {
        if (PowerUpUI != null) PowerUpUI.SetActive(false);
        foreach (var player in players) {
            if (player.CompareTag("PlayerA")) {
                player.gameObject.SetActive(false);
            }
        }
    }
}
