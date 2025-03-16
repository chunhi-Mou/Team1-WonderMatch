public interface IGameMode {
    bool IsPaused { get; }
    void TogglePause();
    void ResetGame();
    void EnterMap();
}
