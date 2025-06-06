﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public interface IGameModeObject {
    void RegisterToGameMode();
}

public class GameModeManager : MonoBehaviour {
    #region Singleton - Dont destroy
    public static GameModeManager instance { get; private set; }

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
    public IGameMode gameMode;
    public bool isPaused = false;
    public bool isProcessingCard = false;
    public bool isUsingPowers = false;
    public bool isMovingCardsInStack = false;
    [SerializeField] GameObject SingleMode;
    [SerializeField] GameObject DuoMode;

    public void OnEnable() {
        GameEvents.OnCardSelected += SetCardProcessingStateTrue;
        GameEvents.OnCardDoneMoving += SetCardProcessingStateFalse;
    }
    private void Start() {
        Audio_PlayBGMusic();
    }
    private void SetCardProcessingStateTrue(Card card = null) => isProcessingCard = true;
    private void SetCardProcessingStateFalse() => isProcessingCard = false;
    public void TogglePause() {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
    }
    public void ResetGame() {
        if (isPaused) TogglePause();
        gameMode.ClearOldData();
        SceneLoader.instance.LoadScene("InGame");
    }
    public void EnterMap() {
        if (isPaused) TogglePause();
        gameMode.ClearOldData();
        SceneLoader.instance.LoadScene("Map");
    }
    public void PauseGame() {
        if (!isPaused) TogglePause();
    }
    public void ResumeGame() {
        if (isPaused) TogglePause();
    }
    public void OnSingleModeSelected() {
        SceneManager.sceneLoaded += OnSceneLoaded;
        if (PlayerPrefs.GetInt("HasSeenTutorial", 0) == 0) {
            PlayerPrefs.SetInt("HasSeenTutorial", 1);
            PlayerPrefs.Save();
            SceneLoader.instance.LoadScene("Tut");
        } else {
            SceneLoader.instance.LoadScene("Map");
        }
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        gameMode = SingleModeManager.instance;

        if (gameMode != null) {
            SingleModeManager.instance.TurnOnObjsOfMode();
        }

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    #region Audio
    private void Audio_PlayBGMusic() {
        AudioManager.instance.Play(SoundEffect.BGMusic);
    }
    #endregion
}
