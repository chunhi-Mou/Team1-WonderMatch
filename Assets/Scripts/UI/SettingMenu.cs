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
        SceneManager.LoadScene("Map");
    }
    public void RestartLevel()
    {
        GameModeManager.instance.gameMode.ResetGame();
    }
}
