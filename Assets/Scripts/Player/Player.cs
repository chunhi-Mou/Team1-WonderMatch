using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    public StackLogic stack;
    public PlayerID playerID;
    private int score = 0;
    private void Awake() {
        this.RegisterToGameMode();
    }
    public void RegisterToGameMode() {
        if (SingleModeManager.instance != null) {
            SingleModeManager.instance.RegisterPlayer(this);
        }
    }
    public void AddScore(CardType cardType)
    {
        score += (int)cardType;
    }
    public int GetPlayerScore()
    {
        return score;
    }
}
