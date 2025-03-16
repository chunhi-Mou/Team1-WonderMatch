using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Board : MonoBehaviour {
    public List<Card> cards = new List<Card>();
    private int currCardCount = 0;
    private void OnEnable() {
        GameEvents.OnMagicPowerClicked += BoardMagicHandler;
        GameEvents.OnShufflePowerClicked += ShuffleBoard;
        GameEvents.OnMatchTilesDone += CheckWinGame;
    }
    private void OnDisable() {
        GameEvents.OnMagicPowerClicked -= BoardMagicHandler;
        GameEvents.OnShufflePowerClicked -= ShuffleBoard;
        GameEvents.OnMatchTilesDone -= CheckWinGame;
    }
    private void Start() {
        UpdateCardsList();
        ShuffleBoard();
        UpdateBoard();
    }
    private void UpdateCardsList() {
        cards = GameObject.FindGameObjectsWithTag("Card")
            .Select(obj => obj.GetComponent<Card>())
            .ToList();
        currCardCount = cards.Count;
    }
    public void ShuffleBoard(CardType cardType = CardType.nothing, int count = 0) {
        List<CardData> cardDataList = GetAllCardsInBoard();
        ShuffleList(cardDataList);
        UpdateBoardCards(cardDataList);

        if (cardType != CardType.nothing && count > 0) {
            SwapSpecificCards(cardType, count);
        }
    }
    private List<CardData> GetAllCardsInBoard() {
        return cards
            .Where(card => card.state == CardState.inBoard)
            .Select(card => card.cardData)
            .ToList();
    }
    private void UpdateBoardCards(List<CardData> cardDataList) {
        int index = 0;
        foreach (var card in cards.Where(c => c.state == CardState.inBoard)) {
            card.cardData = cardDataList[index++];
            card.GetCardData();
        }
    }
    private void SwapSpecificCards(CardType cardType, int count) {
        var selectedCards = cards
            .Where(card => card.state == CardState.inBoard && card.cardData.cardType == cardType)
            .Take(count)
            .ToList();

        var topCards = cards
            .Where(card => card.state == CardState.inBoard)
            .OrderBy(card => card.transform.position.z) //Nhi: Z càng nho -> cang tren cao
            .Take(count)
            .ToList();

        for (int i = 0; i < Mathf.Min(count, selectedCards.Count); i++) {
            (selectedCards[i].cardData, topCards[i].cardData) = (topCards[i].cardData, selectedCards[i].cardData);

            selectedCards[i].GetCardData();
            topCards[i].GetCardData();
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
    public void BoardMagicHandler(CardType cardType, int count) {
        List<Card> availableCards = cards
            .Where(card => card.cardData.cardType == cardType && card.state == CardState.inBoard)
            .OrderBy(_ => Random.value)
            .ToList();
        Debug.Log(cardType);
        Debug.Log(count);
        if (availableCards.Count >= count) {
            List<Card> selectedCards = availableCards.Take(count).ToList();
            foreach (Card card in selectedCards) {
                card.SetSelectableData(true);
                card.PushCardToStack();
            }
        } else {
            Debug.Log($"Not enough {cardType} cards, skipping .-. ");
        }
    }


    private void CheckWinGame() {
        currCardCount -= 3;//Nhi: Match Found sẽ trừ đi 3 Card
        if (currCardCount <= 0) {
            LevelManager.UnlockNextLevel();
            GameEvents.OnWinGameInvoke();
        } 
    }
}
