using System.Collections.Generic;
using UnityEngine;

public class CardHistory {
    public static CardHistory Instance { get; } = new CardHistory();
    private List<CardStateManager> cardHistory = new List<CardStateManager>();

    public void PushCardToHistory(CardStateManager card) {
        cardHistory.Add(card);
    }

    public CardStateManager UndoLastMove() {
        while (cardHistory.Count > 0) {
            CardStateManager lastCard = cardHistory[^1];
            cardHistory.RemoveAt(cardHistory.Count - 1);
            if (lastCard != null && lastCard.gameObject.activeSelf) {
                return lastCard;
            }
        }
        return null;
    }
}
