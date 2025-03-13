using UnityEngine;

public class ShufflePowerUp : IPowerUp {
    private int count = 3;
    private Board board;

    public ShufflePowerUp() {
        board = Object.FindObjectOfType<Board>();
        count = PlayerPrefs.GetInt(SavedData.ShufflePowerCount, 3);
    }

    public void Use() {
        if (count > 0) {
            board.ShuffleBoard();
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
}
