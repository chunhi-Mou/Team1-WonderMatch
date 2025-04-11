using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {
    public Button[] levelButtons;
    public static int CurrLevel = 0;
    #region Singleton - Dont destroy
    public static LevelManager instance { get; private set; }

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }
    }
    #endregion
    public static int UnlockedLevels {
        get => PlayerPrefs.GetInt("UnlockedLevels", 1);
        set {
            PlayerPrefs.SetInt("UnlockedLevels", value);
            PlayerPrefs.Save();
        }
    }

    private void Start() {
        int unlockedLevel = UnlockedLevels;
        for (int i = 0; i < levelButtons.Length; i++) {
            Transform unlockTransform = levelButtons[i].transform.Find("Unlock");

            if (i < unlockedLevel - 1){
                if (unlockTransform != null)
                {
                    GameObject unlockObject = unlockTransform.gameObject;
                    unlockObject.SetActive(true);
                }  
            }
            else {
                if (unlockTransform != null)
                {
                    GameObject unlockObject = unlockTransform.gameObject;
                    unlockObject.SetActive(false);
                }  
            }

            if (i < unlockedLevel){
                levelButtons[i].interactable = true;
            } 
            else {
                levelButtons[i].interactable = false;
            }
        }
    }

    public void CompleteLevel() 
    {
        Transform unlockTransform = levelButtons[CurrLevel - 1].transform.Find("Unlock");
        if (unlockTransform != null)
        {
            //Debug.Log("Level" + CurrLevel + " COMPLETED!");
            GameObject unlockObject = unlockTransform.gameObject;
            unlockObject.SetActive(true);
        }    
    }
    //public void OnClickLevelMenu(int level) {
    //    if (level > UnlockedLevels) return;

    //    levelButtons[level - 1].transform.DOScale(0.95f, 0.1f).OnComplete(() => {
    //        levelButtons[level - 1].transform.DOScale(1f, 0.1f).OnComplete(() => {
    //            if (HeartsSystem.hearts > 0) {
    //                EnterGameLv(level);
    //            } else {
    //                Debug.Log("Not enough Hearts");
    //                GameEvents.OnOutOfHeartInvoke();
    //                levelButtons[level - 1].transform.DOScale(1.2f, 0.2f);
    //            }
    //        });
    //    });

    //}

    public void EnterGameLv(int level) {
        if (level > UnlockedLevels) return;
        if (HeartsSystem.hearts > 0) {
            CurrLevel = level;
            GameModeManager.instance.ResumeGame();
            SceneLoader.instance.LoadScene("InGame");
        } else {
            Debug.Log("Not enough Hearts");
            GameEvents.OnOutOfHeartInvoke();
            levelButtons[level - 1].transform.DOScale(1.1f, 0.2f);
        }
    }

    public static void UnlockNextLevel() {
        if (UnlockedLevels == CurrLevel) {
            UnlockedLevels++;
        }
    }
}