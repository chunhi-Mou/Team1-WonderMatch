using UnityEngine;

public class Board : MonoBehaviour {
    private int currCardCount;

    private void OnEnable() => GameEvents.OnCheckWin += CheckWinGame;
    private void OnDisable() => GameEvents.OnCheckWin -= CheckWinGame;

    private void Start() {
        CardBoardHelper.UpdateCardsList();
        currCardCount = CardBoardHelper.cards.Count;
    }

    private void CheckWinGame() {
        CardBoardHelper.UpdateCardsList();
        currCardCount = CardBoardHelper.cards.Count;
        if (currCardCount <= 0) {
            LevelManager.UnlockNextLevel();
            GameEvents.OnWinGameInvoke();
        }
    }
}
