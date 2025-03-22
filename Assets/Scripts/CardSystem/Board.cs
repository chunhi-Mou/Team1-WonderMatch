using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

public class Board : MonoBehaviour {
    public List<Card> cards = new List<Card>();
    [SerializeField] private Transform centerShufflePoint;
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
        List<Transform> cardTransforms = cards
            .Where(card => card.state == CardState.inBoard)
            .Select(card=> card.gameObject.transform).ToList();

        CardAnimation.PlayCardSpreadAnimation(cardTransforms, centerShufflePoint, 10, 0.5f);
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

        if (availableCards.Count >= count) {
            List<Card> selectedCards = availableCards.Take(count).ToList();

            Sequence sequence = DOTween.Sequence();

            foreach (Card card in selectedCards) {
                sequence.AppendCallback(() => {
                    card.SetSelectableData(true);
                    card.PushCardToStack();
                });

                sequence.AppendInterval(0.1f);

                sequence.AppendCallback(() => {
                    DOVirtual.DelayedCall(0, () => { }, false)
                        .OnUpdate(() => {
                            if (!GameModeManager.instance.isProcessingCard) {
                                sequence.PlayForward();
                            }
                        });
                });
            }
        } else {
            var groupedCards = cards
                .Where(card => card.state == CardState.inBoard)
                .GroupBy(card => card.cardData.cardType)
                .Where(group => group.Count() >= count)
                .OrderBy(_ => Random.value)
                .ToList();

            if (groupedCards.Count > 0) {
                var randomGroup = groupedCards.First();
                List<Card> selectedCards = randomGroup.Take(count).ToList();
                foreach (Card card in selectedCards) {
                    card.SetSelectableData(true);
                    card.PushCardToStack();
                }
            }
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
