using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Board : MonoBehaviour {

    #region Singleton
    public static Board instance { get; private set; }

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }
    }
    #endregion
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
    public void UpdateCardsList() {
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
    public void CheckWinGame() {
        UpdateCardsList();
        if (currCardCount <= 0) {
            LevelManager.UnlockNextLevel();
            GameEvents.OnWinGameInvoke();
        }
    }
}
