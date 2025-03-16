using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static int CurrLevel = 1;
    public void UnlockLevel()
    {
        CurrLevel++;
        if (CurrLevel > PlayerPrefs.GetInt(SavedData.UnlockedLevel, CurrLevel)) {
            PlayerPrefs.SetInt(SavedData.UnlockedLevel, CurrLevel);
            PlayerPrefs.Save();
        }
    }

}
