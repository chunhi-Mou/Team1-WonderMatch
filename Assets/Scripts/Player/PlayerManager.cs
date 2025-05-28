using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] Player playerA;
    [SerializeField] Player playerB;
    public PlayerID playerTurn = PlayerID.A;
    void Start()
    {
        playerA.playerID = PlayerID.A;
        playerB.playerID = PlayerID.B;
        playerB.stack.enabled = false;
        playerA.stack.enabled = true;
    }
    void OnEnable()
    {
        
    }
    void OnDisable()
    {
        
    }
    public void AddScoreToPlayer (CardType cardType)
    {
        if (playerTurn == playerA.playerID) {
            playerA.AddScore(cardType);
        }
        else playerB.AddScore(cardType);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            TurnChange();
        }
    }
    private void TurnChange()
    {
        if (playerTurn == PlayerID.A) {
            playerTurn = PlayerID.B;
            playerA.stack.enabled = false;
            playerB.stack.enabled = true;
        }
        else {
            playerTurn = PlayerID.A;
            playerB.stack.enabled = false;
            playerA.stack.enabled = true;
        }
    }
}
