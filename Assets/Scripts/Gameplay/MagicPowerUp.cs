using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MagicPowerUp : IPowerUp {
    public void OnEnable() {
        GameEvents.OnMagicPowerClicked += BoardMagicHandler;
    }
    public void OnDisable() {
        GameEvents.OnMagicPowerClicked -= BoardMagicHandler;
    }
    private int count = 3;
    private StackLogic stack;
    public static bool canUseMagic = true;

    public MagicPowerUp() {
        stack = GameObject.Find("StackA")?.GetComponent<StackLogic>();
        count = PlayerPrefs.GetInt(SavedData.MagicPowerCount, 3);
    }

    public void Use() {
        if (GameModeManager.instance.isUsingPowers || GameModeManager.instance.isProcessingCard || GameModeManager.instance.isMovingCardsInStack) return;
        if (count > 0) {
            if (stack.StackMagicHandler()) { //Đồng thời Invoke cho Board
                canUseMagic = false;
                GameModeManager.instance.isUsingPowers = true;
                count--;
                SaveData();
            } 
        } else {
            GameEvents.OnSpendCoinsNeededInvoke(PowerType.Magic);
        }
    }

    public void ResetCount(int maxCount) {
        count = maxCount;
    }

    public int GetCount() {
        return count;
    }
    public void SaveData() {
        PlayerPrefs.SetInt(SavedData.MagicPowerCount, count);
        PlayerPrefs.Save();
    }
    public void BoardMagicHandler(CardType cardType, int count) {
        List<Card> availableCards = Board.cards
            .Where(card => card.cardData.cardType == cardType && card.state == CardState.inBoard)
            .OrderBy(_ => Random.value)
            .ToList();

        if (availableCards.Count >= count) {
            SelectAndProcessCards(availableCards.Take(count).ToList());
        } else {
            var groupedCards = Board.cards
                .Where(card => card.state == CardState.inBoard)
                .GroupBy(card => card.cardData.cardType)
                .Where(group => group.Count() >= count)
                .OrderBy(_ => Random.value)
                .ToList();

            if (groupedCards.Count > 0) {
                SelectAndProcessCards(groupedCards.First().Take(count).ToList());
            }
        }
    }
    private void SelectAndProcessCards(List<Card> selectedCards) {
        foreach (Card card in selectedCards) {
            card.SetSelectableData(true);
            card.state = CardState.inStack;
            card.UpdateCardsState();

        }

        GameEvents.OnMatchTilesDone += () => {
            GameModeManager.instance.isUsingPowers = false;
            GameEvents.OnMatchTilesDone -= () => { };
        };
    }

}
