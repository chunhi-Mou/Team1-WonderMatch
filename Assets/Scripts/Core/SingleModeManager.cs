using UnityEngine;

public class SingleModeManager : GameStateBase {
    public static SingleModeManager Instance { get; private set; }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
    public void LoseGame() {
        this.TogglePause();
        //Gọi hàm trừ tim
        Debug.Log("Lose!");
    }
    public void WinGame() {
        this.TogglePause();
        //Gọi Hàm cộng coins
        Debug.Log("Win");
    }
}
