using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {
    public Button[] levelButtons;
    public static int CurrLevel = 0;

    public static int UnlockedLevels {
        get => PlayerPrefs.GetInt("UnlockedLevels", 1);
        set {
            PlayerPrefs.SetInt("UnlockedLevels", value);
            PlayerPrefs.Save();
        }
    }

    private void Start() {
        int unlockedLevel = UnlockedLevels;
        for (int i = 0; i < levelButtons.Length; i++) {
            levelButtons[i].interactable = i < unlockedLevel;
        }
    }

    public void OnClickLevelMenu(int level) {
        if (level > UnlockedLevels) return;

        levelButtons[level - 1].transform.DOScale(0.95f, 0.1f).OnComplete(() => {
            levelButtons[level - 1].transform.DOScale(1f, 0.2f).OnComplete(() => {
                if (HeartsSystem.hearts > 0) {
                    EnterGameLv(level);
                } else {
                    Debug.Log("Not enough Hearts");
                }
            });
        });
    }

    private void EnterGameLv(int level) {
        DOTween.KillAll();
        CurrLevel = level;
        SceneManager.sceneLoaded += OnSceneReloaded;
        SceneManager.LoadScene("InGame");
    }

    private void OnSceneReloaded(Scene scene, LoadSceneMode mode) {
        if (GameModeManager.instance.gameMode == (IGameMode)SingleModeManager.instance) {
            SingleModeManager.instance.TurnOnObjsOfSingleMode();
        }
        SceneManager.sceneLoaded -= OnSceneReloaded;
    }

    public static void UnlockNextLevel() {
        if (UnlockedLevels == CurrLevel) {
            UnlockedLevels++;
        }
    }
}
