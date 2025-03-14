using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingMenu : MonoBehaviour
{
    public static bool GameIsPause = false;
    public GameObject settingMenuUI;
    void Start()
    {
        settingMenuUI.SetActive(false);
    }
    public void Resume()
    {
        settingMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPause = false;
    }
    public void Pause()
    {
        settingMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPause = true;
    }
    public void LoadMenu()
    {
        Debug.Log("Loading menu...");
    }
    public void RestartLevel()
    {
        Debug.Log("Restarting level...");
    }
}
