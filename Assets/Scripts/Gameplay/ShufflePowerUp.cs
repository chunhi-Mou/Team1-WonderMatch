using UnityEngine;
using System.Collections.Generic;


public class ShufflePowerUp : IPowerUp {
    private int count;
    private StackStateManager stack;
    private Transform centerShufflePoint;

    public ShufflePowerUp() {
        stack = GameObject.Find("StackA")?.GetComponent<StackStateManager>();
        count = PlayerPrefs.GetInt(SavedData.ShufflePowerCount, 3);
    }

    public void OnEnable() {
        GetCenterPoint();
        GameEvents.OnShufflePowerClicked += ShuffleBoard;
    }
    public void OnDisable() {
        GameEvents.OnShufflePowerClicked -= ShuffleBoard;
    }

    public void Use() {
        if (count > 0) {
            //stack.ShuffleMagicHandler();
            count--;
            SaveData();
        } else {
            GameEvents.OnSpendCoinsNeededInvoke(PowerType.Shuffle);
        }
    }

    public int GetCount() => count;
    public void ResetCount(int maxCount) => count = maxCount;

    public void SaveData() {
        PlayerPrefs.SetInt(SavedData.ShufflePowerCount, count);
        PlayerPrefs.Save();
    }
    public void ShuffleBoard(CardType cardType = CardType.nothing, int swapCount = 0) {
        var cardDataList = CardBoardHelper.GetAllCardNotInTray();
        var cardTransforms = CardBoardHelper.GetAllCardNotInTrayTransforms();

        CardAnimation.PlayCardSpreadAnimation(cardTransforms, centerShufflePoint, 20, 0.8f);
        ShuffleList(cardDataList);
        CardBoardHelper.UpdateBoardCards(cardDataList);

        if (cardType != CardType.nothing && swapCount > 0) {
            CardBoardHelper.SwapSpecificCards(cardType, swapCount);
        }
    }
    public void ShuffleList<T>(List<T> list) {
        for (int i = list.Count - 1; i > 0; i--) {
            int randomIndex = Random.Range(0, i + 1);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }
    private void GetCenterPoint() => centerShufflePoint = new GameObject("CenterPoint").transform;

}
