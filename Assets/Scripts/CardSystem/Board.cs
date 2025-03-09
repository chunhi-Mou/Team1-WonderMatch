using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Board : MonoBehaviour {
    public List<Card> cards = new List<Card>();
    private int currCardCount = 0;
    private void Start() {
        UpdateCardsList();
        ShuffleBoard();
        UpdateBoard();
    }
    private void UpdateCardsList() {
        cards = GameObject.FindGameObjectsWithTag("Card").Select(obj => obj.GetComponent<Card>()).ToList();
        currCardCount = cards.Count;
    }
    public void ShuffleBoard() {
        List<CardData> cardDataList = cards.Select(card => card.cardData).ToList();
        ShuffleList(cardDataList);

        for (int i = 0; i < cards.Count; i++) {
            cards[i].cardData = cardDataList[i];
            cards[i].GetCardData();
        }
    }

    public void UpdateBoard() {
        foreach (var card in cards) {
            CardOverlapChecker checker = card.gameObject.GetComponent<CardOverlapChecker>();
            checker.CheckIfUncovered();
        }
    }
    private void ShuffleList<T>(List<T> list) {
        for (int i = list.Count - 1; i > 0; i--) {
            int randomIndex = Random.Range(0, i + 1);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }
    private void CheckWinGame() {
        currCardCount -= 3;//Nhi: Match Found sẽ trừ đi 3 Card
        if (currCardCount <= 0) {
            //WinGame
        }
    }
}
