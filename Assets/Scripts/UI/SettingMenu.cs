using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Debug.Log("Loading menu...");
    }
    public void RestartLevel()
    {
        GameModeManager.instance.gameMode.ResetGame();
    }
}
