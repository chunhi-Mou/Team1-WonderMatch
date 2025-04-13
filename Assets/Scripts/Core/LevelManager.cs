using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {
    public ButtonLvObj[] levelButtons;
    [SerializeField] private Sprite fullStar;
    [SerializeField] private Sprite emptyStar;

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
        UpdateLevelStars();
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
                levelButtons[i].levelButton.interactable = true;
            } 
            else {
                levelButtons[i].levelButton.interactable = false;
            }

            
        }
    }
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
    private void UpdateLevelStars() {
        for (int i = 0; i < levelButtons.Length; i++) {
            int starsEarned = PlayerPrefs.GetInt("LevelStars_" + (i + 1), 0);
            levelButtons[i].UpdateStars(starsEarned, fullStar, emptyStar);
        }
    }
    public static void UnlockNextLevel() {
        if (UnlockedLevels == CurrLevel) {
            UnlockedLevels++;
        }
    }
}