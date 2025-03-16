using DG.Tweening;
using TMPro;
using UnityEngine;

public class HeartsSystem : MonoBehaviour {
    [Header("Text")]
    [SerializeField] TextMeshProUGUI heartsCountTxt;
    [SerializeField] TextMeshProUGUI healTimeTxt;

    [Header("UI")]
    [SerializeField] RectTransform HeartIcon;
    [SerializeField] RectTransform AdsButton;

    [Header("Data")]
    public int maxHearts = 3;
    public float healTime = 300f;

    public static int hearts;
    private static System.DateTime lastHealTime;

    public static HeartsSystem instance;
    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }
    }

    private void Start() {
        LoadHeartsData();
    }
    private void OnEnable() {
        GameEvents.OnLoseGame += LoseHeart;
    }
    private void OnDisable() {
        GameEvents.OnLoseGame -= LoseHeart;
    }
    private void Update() {
        ShowUpHearts();
        HealHeartInGame();
        if (Input.GetKeyDown(KeyCode.A) ){
            LoseHeart();
        }
        if (Input.GetKeyDown(KeyCode.S)) {
            ClearPlayerPrefs();
        }
    }

    private void ShowUpHearts() {
        heartsCountTxt.text = hearts.ToString();

        if (hearts < maxHearts) {
            if (hearts == 0) AdsButton.gameObject.SetActive(true);

            double timePassed = (System.DateTime.UtcNow - lastHealTime).TotalSeconds;
            float timeLeft = Mathf.Max(0, healTime - (float)timePassed);
            int minutes = Mathf.FloorToInt(timeLeft / 60);
            int seconds = Mathf.FloorToInt(timeLeft % 60);
            healTimeTxt.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        } else {
            healTimeTxt.text = "FULL";
            AdsButton.gameObject.SetActive(false);
        }
    }

    private void LoadHeartsData() {
        hearts = PlayerPrefs.GetInt("Hearts", maxHearts);
        lastHealTime = System.DateTime.Parse(PlayerPrefs.GetString("LastHealTimestamp", System.DateTime.UtcNow.ToString()));

        float timePassed = (float) (System.DateTime.UtcNow - lastHealTime).TotalSeconds;
        int heartsToHeal = Mathf.FloorToInt(timePassed / healTime);

        if (heartsToHeal > 0) {
            hearts = Mathf.Min(maxHearts, hearts + heartsToHeal);
            lastHealTime = System.DateTime.UtcNow.AddSeconds(-timePassed % healTime);
        }

        SaveHearts();
    }

    private void HealHeartInGame() {
        if (hearts < maxHearts) {
            double timePassed = (System.DateTime.UtcNow - lastHealTime).TotalSeconds;
            if (timePassed >= healTime) {
                HealHeart(1);
            }
        }
    }
    private void HealHeart(int _hearts) {
        hearts = Mathf.Min(maxHearts, hearts + _hearts);
        lastHealTime = System.DateTime.UtcNow;
        SaveHearts();
    }
    public void WatchAdHearts() {
        Debug.Log("WATCH AD");
        HealHeart(maxHearts);
    }
    public void LoseHeart() {
        hearts = Mathf.Max(0, hearts - 1);
        lastHealTime = System.DateTime.UtcNow;
        SaveHearts();
    }

    private void SaveHearts() {
        PlayerPrefs.SetInt("Hearts", hearts);
        PlayerPrefs.SetString("LastHealTimestamp", lastHealTime.ToString());
        PlayerPrefs.Save();
    }
    public static void ClearPlayerPrefs() {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("All PlayerPrefs cleared.");
    }
}
