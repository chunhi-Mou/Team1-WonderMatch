using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LevelMenu : MonoBehaviour
{
    public Button[] buttons;
    private void Awake()
    {
        int unlockedLevel = PlayerPrefs.GetInt(SavedData.UnlockedLevel, 1); 
        for (int i=0; i<buttons.Length; i++){
            buttons[i].interactable = false;
        }
        for (int i=0; i<unlockedLevel; i++){
            buttons[i].interactable = true;
        }
    }
    public void OpenLevel(int levelID)
    {
        SceneManager.sceneLoaded += OnSceneReloaded;
        SceneManager.LoadScene(2);
        if (GameModeManager.instance.gameMode == (IGameMode)SingleModeManager.instance)
            SingleModeManager.instance.TurnOnObjsOfSingleMode();
    }
    private void OnSceneReloaded(Scene scene, LoadSceneMode mode) {
        if (GameModeManager.instance.gameMode == (IGameMode)SingleModeManager.instance)
            SingleModeManager.instance.TurnOnObjsOfSingleMode();
        SceneManager.sceneLoaded -= OnSceneReloaded;
    }
}
