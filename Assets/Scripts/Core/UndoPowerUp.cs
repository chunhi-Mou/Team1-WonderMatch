using UnityEngine;

public class UndoPowerUp : IPowerUp {
    private int count = 3;
    public UndoPowerUp() {
        count = PlayerPrefs.GetInt(SavedData.UndoPowerCount, 3);
    }
    public void Use() {
        if (count > 0) {
            Card lastCard = CardHistory.Instance.UndoLastMove();
            if (lastCard != null) {
                lastCard.UndoMove();
                count--;
                SaveData();
            }
        } else {
            GameEvents.OnSpendCoinsNeededInvoke(PowerType.Undo);
        }
    }

    public void ResetCount(int maxCount) {
        count = maxCount;
    }

    public int GetCount() {
        return count;
    }
    public void SaveData() {
        PlayerPrefs.SetInt(SavedData.UndoPowerCount, count);
        PlayerPrefs.Save();
    }
}
