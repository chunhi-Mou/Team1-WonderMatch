using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingMenu : MonoBehaviour
{
    public GameObject settingMenuUI;
    void Start()
    {
        settingMenuUI.SetActive(false);
    }
    public void Resume()
    {
        settingMenuUI.SetActive(false);
        GameModeManager.instance.gameMode.TogglePause();
    }
    public void Pause()
    {
        settingMenuUI.SetActive(true);
        GameModeManager.instance.gameMode.TogglePause();
    }
    public void LoadMenu()
    {
        GameModeManager.instance.gameMode.EnterMap();
    }
    public void RestartLevel()
    {
        GameModeManager.instance.gameMode.ResetGame();
    }
    public void NextLevel() {
        LevelManager.UnlockNextLevel();
        LevelManager.CurrLevel++;
        RestartLevel();
    }
    public void QuitMidGame() {
        LoadMenu();
        HeartsSystem.instance.LoseHeart();
    }
}
