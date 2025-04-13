using UnityEngine;
using UnityEngine.UI;

public class StarsSystems : MonoBehaviour {
    [Header("Sprites")]
    [SerializeField] private Image[] starImages;
    [SerializeField] private Sprite fullStar;
    [SerializeField] private Sprite emptyStar;

    public static int stars = 0;
    #region Singleton
    public static StarsSystems instance;
    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }
    }
    #endregion
    public void ShowUpStars() {
        for (int i = 0; i < starImages.Length; i++) {
            starImages[i].sprite = (i < stars) ? fullStar : emptyStar; 
        }
    }

    public void SaveStarsData() {
        SaveStars(LevelManager.CurrLevel, stars);
        this.ShowUpStars();
    }
    public void SaveStars(int level, int newStars) {
        string key = "LevelStars_" + level;
        int oldStars = PlayerPrefs.GetInt(key, 0);
        if (newStars > oldStars) {
            PlayerPrefs.SetInt(key, newStars);
            PlayerPrefs.Save();
        }
        this.ShowUpStars();
    }
}
