using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class GameStateBase : MonoBehaviour {
    protected bool isPaused = false;

    public virtual void TogglePause() {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
    }

    public virtual void ResetGame() {
        Time.timeScale = 1;
        isPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
