using UnityEngine;

public class AddOneCellPowerUp : IPowerUp {
    public void OnEnable() {
    }
    public void OnDisable() {
    }
    private int count = 3;
    private StackLogic stack;
    private GameObject addOneCellObj;

    public AddOneCellPowerUp() {
        count = PlayerPrefs.GetInt(SavedData.AddOneCellPowerCount, 3);
        stack = Object.FindObjectOfType<StackLogic>();
        addOneCellObj = GameObject.Find("AddOneCellObj");
    }

    public void Use() {
        stack.AddOneCell();
        if (addOneCellObj != null) {
            addOneCellObj.SetActive(false);
            count--;
            SaveData();
        }
    }

    public void ResetCount(int maxCount) {
        count = maxCount;
    }
    public int GetCount() {
        return count;
    }
    public void SaveData() {
        PlayerPrefs.SetInt(SavedData.AddOneCellPowerCount, count);
        PlayerPrefs.Save();
    }
}
