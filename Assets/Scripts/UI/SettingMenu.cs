using UnityEngine;

public class SettingMenu : MonoBehaviour
{
    public GameObject settingMenuUI;
    public GameObject settingButton;
    void Start()
    {
        settingMenuUI.SetActive(false);
        settingButton.SetActive(true);
    }
    public void Resume()
    {
        settingMenuUI.SetActive(false);
        settingButton.SetActive(true);
        GameModeManager.instance.ResumeGame();
    }
    public void Pause()
    {
        settingMenuUI.SetActive(true);
        settingButton.SetActive(false);
        GameModeManager.instance.PauseGame();
    }
    public void LoadMenu()
    {
        GameModeManager.instance.EnterMap();
    }
    public void RestartLevel()
    {
        GameModeManager.instance.ResetGame();
    }
    public void NextLevel() {
        LevelManager.UnlockNextLevel();
        LevelManager.CurrLevel++;
        RestartLevel();
    }
    public void QuitMidGame() {
        LoadMenu();
        HeartsSystem.LoseHeart();
    }
}
