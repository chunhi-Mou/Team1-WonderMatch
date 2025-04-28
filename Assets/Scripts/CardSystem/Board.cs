using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Board : MonoBehaviour {
    public static List<Card> cards = new List<Card>();
    
    public static int currCardCount = 0;
    private void OnEnable() {
        GameEvents.OnMatchTilesDone += CheckWinGame;
    }
    private void OnDisable() {
        GameEvents.OnMatchTilesDone -= CheckWinGame;
    }
    private void Start() {
        UpdateCardsList();
        UpdateBoard();
    }
    private void UpdateCardsList() {
        cards = GameObject.FindGameObjectsWithTag("Card")
            .Select(obj => obj.GetComponent<Card>())
            .ToList();
        currCardCount = cards.Count;
    }
    
    public void UpdateBoard() {
        foreach (var card in cards) {
            card.GetComponent<CardOverlapChecker>().CheckIfUncovered();
        }
    }
    private void CheckWinGame() {
        currCardCount -= 3; // Match Found sẽ trừ đi 3 Card
        if (currCardCount <= 0) {
            LevelManager.UnlockNextLevel();
            GameEvents.OnWinGameInvoke();
        }
    }
}
