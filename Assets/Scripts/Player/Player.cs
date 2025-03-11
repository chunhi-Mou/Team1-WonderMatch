using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    public Stack stack;
    public PlayerID playerID;
    private int score = 0;
    public void AddScore(CardType cardType)
    {
        score += (int)cardType;
    }
    public int GetPlayerScore()
    {
        return score;
    }
}
