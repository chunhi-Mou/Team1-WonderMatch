using UnityEngine;

public class Board : MonoBehaviour {
    private int currCardCount;

    private void OnEnable() => GameEvents.OnMatchTilesDone += CheckWinGame;
    private void OnDisable() => GameEvents.OnMatchTilesDone -= CheckWinGame;

    private void Start() {
        BoardController.UpdateCardsList();
        currCardCount = BoardController.cards.Count;
    }

    private void CheckWinGame() {
        currCardCount -= 3; // Mỗi lần match thành công sẽ giảm 3 card
        if (currCardCount <= 0) {
            LevelManager.UnlockNextLevel();
            GameEvents.OnWinGameInvoke();
        }
    }
}
