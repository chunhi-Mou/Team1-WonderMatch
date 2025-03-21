using UnityEngine;

public class MagicPowerUp : IPowerUp {
    private int count = 3;
    private StackLogic stack;

    public MagicPowerUp() {
        stack = GameObject.Find("StackA")?.GetComponent<StackLogic>();
        count = PlayerPrefs.GetInt(SavedData.MagicPowerCount, 3);
    }

    public void Use() {
        if (count > 0) {
            if (stack.StackMagicHandler()) { //Đồng thời Invoke cho Board
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
}
