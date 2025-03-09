using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour {
    public Board board;
    public Stack stack;
    private Stack<Card> cardHistory = new Stack<Card>();
    private void Awake() {
        if (board == null) {
            board = FindObjectOfType<Board>(); 
        }
        if (stack == null) {
            stack = FindObjectOfType<Stack>();
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
            Card lastCard = null;
            while(lastCard==null)
                lastCard = cardHistory.Pop();
            lastCard.UndoMove();
        } else {
            Debug.Log("No Card Left!");
        }
    }
    public void OnMagicPress() {
        stack.StackMagicHandler();
    }
}
