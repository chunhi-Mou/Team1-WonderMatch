﻿using UnityEngine;

public class AddOneCellPowerUp : IPowerUp {
    public void OnEnable() {
    }
    public void OnDisable() {
    }
    private int count = 3;
    private StackLogic stack;
    private GameObject addCell;

    public AddOneCellPowerUp() {
        PowerUpsManager.Instance.cellImageObj.SetActive(false);
        addCell = PowerUpsManager.Instance.addCellObj;
        count = PlayerPrefs.GetInt(SavedData.AddOneCellPowerCount, 3);
        stack = GameObject.Find("StackA").GetComponent<StackLogic>();
        count = PlayerPrefs.GetInt(SavedData.AddOneCellPowerCount, 3);
    }

    public void Use() {
        if (count > 0) {
            stack.AddOneCell();
            count--;
            SaveData();
            GameModeManager.instance.isUsingPowers = true;
            CustomAnimation.PlayExitAnimation(addCell.transform, () => {
                addCell.SetActive(false);
                PowerUpsManager.Instance.cellImageObj.SetActive(true);
                GameModeManager.instance.isUsingPowers = false;
            });
        } else {
            GameEvents.OnSpendCoinsNeededInvoke(PowerType.AddOneCell);
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
