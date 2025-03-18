using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour {
    public Button[] buttons;

    private void Awake() {
        int unlockedLevel = LevelManager.UnlockedLevels;
        for (int i = 0; i < buttons.Length; i++) {
            buttons[i].interactable = i < unlockedLevel;
        }
    }

    public void OpenLevel(int levelID) {
        if (levelID > LevelManager.UnlockedLevels) return;

        SceneManager.sceneLoaded += OnSceneReloaded;
        SceneManager.LoadScene("InGame");
    }

    private void OnSceneReloaded(Scene scene, LoadSceneMode mode) {
        if (GameModeManager.instance.gameMode == (IGameMode)SingleModeManager.instance) {
            SingleModeManager.instance.TurnOnObjsOfSingleMode();
        }
        SceneManager.sceneLoaded -= OnSceneReloaded;
    }
}
