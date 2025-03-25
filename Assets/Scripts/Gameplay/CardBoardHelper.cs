using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class CardBoardHelper {
    public static List<CardStateManager> cards { get; private set; } = new();

    public static void UpdateCardsList() {
        cards = GameObject.FindGameObjectsWithTag("Card")
            .Select(obj => obj.GetComponent<CardStateManager>())
            .Where(c => c != null)
            .ToList();
    }

    public static List<CardData> GetAllCardNotInTray() =>
        cards.Where(c => c.currState == c.idleState || c.currState == c.blockedState)
             .Select(c => c.cardInfo.cardData)
             .ToList();

    public static List<Transform> GetAllCardNotInTrayTransforms() =>
        cards.Where(c => c.currState == c.idleState || c.currState == c.blockedState)
             .Select(c => c.transform)
             .ToList();

    public static void UpdateBoardCards(List<CardData> dataList) {
        var idleCards = cards.Where(c => c.currState == c.idleState || c.currState == c.blockedState).ToList();
        for (int i = 0; i < idleCards.Count && i < dataList.Count; i++) {
            idleCards[i].cardInfo.SetCardData(dataList[i].cardType);
        }
    }

    public static void SwapSpecificCards(CardType cardType, int count) {
        var selected = cards.Where(c => (c.currState == c.idleState || c.currState == c.blockedState) && c.cardInfo.cardData.cardType == cardType)
                            .Take(count)
                            .ToList();

        var topCards = cards.Where(c => (c.currState == c.idleState || c.currState == c.blockedState))
                            .OrderBy(c => c.transform.position.z)
                            .Take(count)
                            .ToList();

        for (int i = 0; i < Mathf.Min(count, selected.Count); i++) {
            (selected[i].cardInfo.cardData, topCards[i].cardInfo.cardData) =
            (topCards[i].cardInfo.cardData, selected[i].cardInfo.cardData);

            selected[i].cardInfo.SetCardData(selected[i].cardInfo.cardData.cardType);
            topCards[i].cardInfo.SetCardData(topCards[i].cardInfo.cardData.cardType);
        }
    }

    public static List<CardStateManager> GetAvailableCards(CardType cardType, int count) {
        var matchingCards = cards
            .Where(c => c.cardInfo.cardData.cardType == cardType && c.currState == c.idleState || c.currState == c.blockedState)
            .OrderBy(_ => Random.value)
            .Take(count)
            .ToList();

        return matchingCards.Count == count ? matchingCards : GetFallbackCards(count);
    }

    private static List<CardStateManager> GetFallbackCards(int count) =>
        cards.Where(c => c.currState == c.idleState || c.currState == c.blockedState)
             .GroupBy(c => c.cardInfo.cardData.cardType)
             .Where(g => g.Count() >= count)
             .OrderBy(_ => Random.value)
             .SelectMany(g => g.Take(count))
             .Take(count)
             .ToList();
}