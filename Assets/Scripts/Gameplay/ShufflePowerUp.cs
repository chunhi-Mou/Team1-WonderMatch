using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using DG.Tweening;


public class ShufflePowerUp : IPowerUp {
    public void OnEnable() {
        GetCenterPoint();
        GameEvents.OnShufflePowerClicked += HandleShufflePowerClicked;
        DOVirtual.DelayedCall(0.2f, () => ShuffleBoardWhenStart(CardType.nothing, 0, () => GameEvents.StartTimer())).SetAutoKill(true);
    }
    public void OnDisable() {
        GameEvents.OnShufflePowerClicked -= HandleShufflePowerClicked;
    }
    private int count = 3;
    private StackLogic stack;
    [SerializeField] private Transform centerShufflePoint;

    public ShufflePowerUp() {
        stack = GameObject.Find("StackA")?.GetComponent<StackLogic>();
        count = PlayerPrefs.GetInt(SavedData.ShufflePowerCount, 3);
    }

    public void Use() {
        if(GameModeManager.instance.isUsingPowers) return;
        if (count > 0 && GetAllCardsInBoard().Count > 1) {
            stack.ShuffleMagicHandler();
            count--;
            SaveData();
        } else {
            GameEvents.OnSpendCoinsNeededInvoke(PowerType.Shuffle);
        }
    }

    public void ResetCount(int maxCount) {
        count = maxCount;
    }

    public int GetCount() {
        return count;
    }
    public void SaveData() {
        PlayerPrefs.SetInt(SavedData.ShufflePowerCount, count);
        PlayerPrefs.Save();
    }
    private void HandleShufflePowerClicked(CardType cardType = CardType.nothing, int count = 0) {
        ShuffleBoard(cardType, count);
    }
    public void ShuffleBoard(CardType cardType = CardType.nothing, int count = 0, System.Action onComplete = null) {
        List<CardData> cardDataList = GetAllCardsInBoard();
        List<Transform> cardTransforms = Board.cards
            .Where(card => card.state == CardState.inBoard)
            .Select(card => card.gameObject.transform)
            .ToList();

        CardAnimation.PlayCardSpreadAnimation(cardTransforms, centerShufflePoint, 20, 1f, onComplete);
        ShuffleList(cardDataList);
        UpdateBoardCards(cardDataList);

        if (cardType != CardType.nothing && count > 0) {
            SwapSpecificCards(cardType, count);
        }
    }
    public void ShuffleBoardWhenStart(CardType cardType = CardType.nothing, int count = 0, System.Action onComplete = null) {
        List<CardData> cardDataList = GetAllCardsInBoard();
        List<Transform> cardTransforms = Board.cards
            .Where(card => card.state == CardState.inBoard)
            .Select(card => card.gameObject.transform)
            .ToList();

        CardAnimation.StartShuffleCardsAnimations(cardTransforms, centerShufflePoint, 20, 1f, onComplete);
        ShuffleList(cardDataList);
        UpdateBoardCards(cardDataList);

        if (cardType != CardType.nothing && count > 0) {
            SwapSpecificCards(cardType, count);
        }
    }
    private List<CardData> GetAllCardsInBoard() {
        return Board.cards
            .Where(card => card.state == CardState.inBoard)
            .Select(card => card.cardData)
            .ToList();
    }
    private void UpdateBoardCards(List<CardData> cardDataList) {
        int index = 0;
        foreach (var card in Board.cards.Where(c => c.state == CardState.inBoard)) {
            card.cardData = cardDataList[index++];
            card.GetCardData();
        }
    }
    private void SwapSpecificCards(CardType cardType, int count) {
        var selectedCards = Board.cards
            .Where(card => card.state == CardState.inBoard && card.cardData.cardType == cardType)
            .Take(count)
            .ToList();

        var topCards = Board.cards
            .Where(card => card.state == CardState.inBoard)
            .OrderBy(card => card.transform.position.z)
            .Take(count)
            .ToList();

        for (int i = 0; i < Mathf.Min(count, selectedCards.Count); i++) {
            (selectedCards[i].cardData, topCards[i].cardData) = (topCards[i].cardData, selectedCards[i].cardData);

            selectedCards[i].GetCardData();
            topCards[i].GetCardData();
        }
    }
    private void ShuffleList<T>(List<T> list) {
        for (int i = list.Count - 1; i > 0; i--) {
            int randomIndex = Random.Range(0, i + 1);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }
    private void GetCenterPoint() => centerShufflePoint = new GameObject("CenterPoint").transform;

}
