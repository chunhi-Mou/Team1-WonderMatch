using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Board : MonoBehaviour {
    private void Start() {
        ShuffleBoard();
        UpdateBoard();
    }
    public void ShuffleBoard() {
        List<Card> cards = GameObject.FindGameObjectsWithTag("Card")
                                 .Select(obj => obj.GetComponent<Card>())
                                 .ToList();

        List<CardData> cardDataList = cards.Select(card => card.cardData).ToList();
        ShuffleList(cardDataList);

        for (int i = 0; i < cards.Count; i++) {
            cards[i].cardData = cardDataList[i];
            cards[i].GetCardData();
        }
    }

    public void UpdateBoard() {
        GameObject[] cards = GameObject.FindGameObjectsWithTag("Card");
        foreach (GameObject card in cards) {
            CardOverlapChecker checker = card.GetComponent<CardOverlapChecker>();
            checker.CheckIfUncovered();
        }
    }
    private void ShuffleList<T>(List<T> list) {
        for (int i = list.Count - 1; i > 0; i--) {
            int randomIndex = Random.Range(0, i + 1);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }
}
