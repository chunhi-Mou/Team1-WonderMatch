using DG.Tweening;
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
    }
    #endregion

    private void OnEnable() {
        RegisterEvents();
    }
    private void OnDisable() {
        UnregisterEvents();
    }
    private void RegisterEvents() {
        GameEvents.OnLoseGame += TogglePause;
        GameEvents.OnWinGame += TogglePause;
    }
    private void UnregisterEvents() {
        GameEvents.OnLoseGame -= TogglePause;
        GameEvents.OnWinGame -= TogglePause;
    }
    public void TogglePause() {
        GameModeManager.instance.PauseGame();
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
    public void ClearOldData() {
        DOTween.KillAll();
        players.Clear();
    }
    public void TurnOnObjsOfMode() {
        if (PowerUpUI != null) PowerUpUI.SetActive(true);
        foreach (var player in players) {
            if (player.CompareTag("PlayerA")) { //Nhi: Player A is default
                player.gameObject.SetActive(true);
            } else {
                player.gameObject.SetActive(false);
            }
        }
    }
    public void TurnOffObjsOfSingleMode() {
        if (PowerUpUI != null) PowerUpUI.SetActive(false);
        foreach (var player in players) {
            player.gameObject.SetActive(false);
        }
    }
}
