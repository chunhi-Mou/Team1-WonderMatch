using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour {
    [SerializeField] GameObject addOneCellObj;
    public Board board;
    public Stack stack;
    private List<Card> cardHistory = new List<Card>();
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
    public void OnAddOneCell(){
        stack.AddOneCell();
        addOneCellObj.SetActive(false);
    }
    public void PushCardToHistory(Card card) {
        cardHistory.Add(card);
    }
    public void OnUndoPress() {
        Card lastCard = null;
        while (cardHistory.Count > 0) {
            lastCard = cardHistory[^1];
            cardHistory.RemoveAt(cardHistory.Count - 1);
            if (lastCard != null && lastCard.gameObject.activeSelf) {
                break;
            }
        }
        if (lastCard != null && lastCard.gameObject.activeSelf) {
            lastCard.UndoMove();
        } else {
            Debug.Log("No Card Left!");
        }
    }

    public void OnMagicPress() {
        stack.StackMagicHandler();
    }
}
