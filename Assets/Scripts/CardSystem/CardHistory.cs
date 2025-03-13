using System.Collections.Generic;
using UnityEngine;

public class CardHistory {
    public static CardHistory Instance { get; } = new CardHistory();
    private List<Card> cardHistory = new List<Card>();

    public void PushCardToHistory(Card card) {
        cardHistory.Add(card);
    }

    public Card UndoLastMove() {
        while (cardHistory.Count > 0) {
            Card lastCard = cardHistory[^1];
            cardHistory.RemoveAt(cardHistory.Count - 1);
            if (lastCard != null && lastCard.gameObject.activeSelf) {
                return lastCard;
            }
        }
        return null;
    }
}
