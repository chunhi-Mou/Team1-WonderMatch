using UnityEngine;
using UnityEngine.SceneManagement;

public class DuoModeManager : MonoBehaviour, IGameMode {
    #region Singleton - Dont destroy
    public static DuoModeManager instance { get; private set; }

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }
    }
    #endregion
    private bool isPaused = false;
    public bool IsPaused => isPaused;

    public void TogglePause() {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
    }

    public void ResetGame() {
        Time.timeScale = 1;
        isPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
