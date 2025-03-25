using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class MagicPowerUp : IPowerUp {
    private int count;
    private StackStateManager stack;

    public MagicPowerUp() {
        stack = GameObject.Find("StackA")?.GetComponent<StackStateManager>();
        count = PlayerPrefs.GetInt(SavedData.MagicPowerCount, 3);
    }

    public void OnEnable() => GameEvents.OnMagicPowerClicked += BoardMagicHandler;
    public void OnDisable() => GameEvents.OnMagicPowerClicked -= BoardMagicHandler;
    public void Use() {
        if (count > 0) {
            //if (stack.StackMagicHandler()) { //Đồng thời Invoke cho Board
            //    count--;
            //    SaveData();
            //} 
        } else {
            GameEvents.OnSpendCoinsNeededInvoke(PowerType.Magic);
        }
    }

    public int GetCount() => count;
    public void ResetCount(int maxCount) => count = maxCount;
    public void SaveData() {
        PlayerPrefs.SetInt(SavedData.MagicPowerCount, count);
        PlayerPrefs.Save();
    }
    public void BoardMagicHandler(CardType cardType, int selectCount) {
        var selectedCards = BoardController.GetAvailableCards(cardType, selectCount);
        ProcessSelectedCards(selectedCards);
    }
    public static void ProcessSelectedCards(List<CardStateManager> selectedCards) {
        var sequence = DOTween.Sequence();

        foreach (var card in selectedCards) {
            sequence.AppendCallback(() => {
                card.SwitchState(card.movingState);
            });

            sequence.AppendInterval(0.1f);
        }
    }
}
