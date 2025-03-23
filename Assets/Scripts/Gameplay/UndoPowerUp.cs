using UnityEngine;

public class UndoPowerUp : IPowerUp {
    public void OnEnable() {
    }
    public void OnDisable() {
    }
    private int count = 3;
    public UndoPowerUp() {
        count = PlayerPrefs.GetInt(SavedData.UndoPowerCount, 3);
    }
    public void Use() {
        if (count > 0) {
            Card lastCard = CardHistory.Instance.UndoLastMove();
            if (lastCard != null) {
                AudioManager.instance.Play(SoundEffect.Undo);
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
