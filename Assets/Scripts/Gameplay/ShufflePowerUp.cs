using UnityEngine;

public class ShufflePowerUp : IPowerUp {
    private int count = 3;
    private Stack stack;

    public ShufflePowerUp() {
        stack = GameObject.Find("StackA")?.GetComponent<Stack>();
        count = PlayerPrefs.GetInt(SavedData.ShufflePowerCount, 3);
    }

    public void Use() {
        if (count > 0) {
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
}
