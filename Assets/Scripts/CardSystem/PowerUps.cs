using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour {
    public Board board;
    private Stack<Card> cardHistory = new Stack<Card>();
    private void Awake() {
        if (board == null) {
            board = FindObjectOfType<Board>(); 
        }
    }

    private void OnEnable() {
        GameEvents.OnCardSelected += PushCardToHistory;
    }
    private void OnDisable() {
        GameEvents.OnCardSelected -= PushCardToHistory;
    }
    public void OnShufflePress() {
        board.ShuffleBoard();
    }
    public void PushCardToHistory(Card card) {
        cardHistory.Push(card);
    }
    public void OnUndoPress() {
        if (cardHistory.Count > 0) {
            Card lastCard = cardHistory.Pop();
            lastCard.UndoMove();
        } else {
            Debug.Log("No Card Left!");
        }
    }
}
