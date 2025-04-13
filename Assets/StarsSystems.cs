using DG.Tweening;
using Unity.VisualScripting;
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
            Image star = starImages[i];
            bool isFull = (i < stars);

            star.sprite = isFull ? fullStar : emptyStar;
            star.transform.DOKill(true);

            if (isFull) {
                star.transform.localScale = Vector3.zero;
                star.color = new Color(1, 1, 1, 0);
                DG.Tweening.Sequence seq = DOTween.Sequence();
                seq.SetUpdate(true); 
                seq.Append(star.transform.DOScale(1.9f, 0.5f).SetEase(Ease.OutBack));
                seq.Join(star.DOFade(1f, 0.2f)); 
                seq.Join(star.transform.DORotate(new Vector3(0, 0, 360), 0.5f, RotateMode.FastBeyond360));
                seq.Append(star.transform.DOScale(1f, 0.2f));
                seq.SetDelay(i * 0.1f);
            } else {
                star.transform.localScale = Vector3.one;
                star.color = Color.white;
                star.transform.rotation = Quaternion.identity;
            }
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
