using TMPro;
using UnityEngine;

public class CoinsManager : MonoBehaviour {
    public int currCoins { get; private set; }
    public static int powerCost = 100;
    [SerializeField] private TextMeshProUGUI coinsText;

    #region Singleton
    public static CoinsManager Instance { get; private set; }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
            return;
        }
    }
    #endregion
    private void Start() {
        this.RestoreCoins();
    }
    private void OnEnable() {
        GameEvents.OnWinGame += AddCoinsOnWin;
    }
    private void OnDisable() {
        GameEvents.OnWinGame -= AddCoinsOnWin;
    }
    public void AddCoinsOnWin() {
        this.AddCoins(20);
    }
    private void RestoreCoins() {
        currCoins = PlayerPrefs.GetInt(SavedData.Coins, 0);
        UpdateUI();
        GameEvents.OnCoinsChangedInvoke(currCoins);
    }
    public void AddCoins(int _coins) {
        currCoins += _coins;
        SaveCoinsData(currCoins);
    }
    public bool TrySpendCoins() {
        if (currCoins < powerCost) return false;
        currCoins -= powerCost;
        SaveCoinsData(currCoins);
        return true;
    }
    private void SaveCoinsData(int _coins) {
        PlayerPrefs.SetInt(SavedData.Coins, _coins);
        PlayerPrefs.Save();
        GameEvents.OnCoinsChangedInvoke(_coins);
        UpdateUI();
    }
    public bool CanBuyPower() {
        return currCoins >= powerCost;
    }
    private void UpdateUI() {
        if (coinsText != null) {
            coinsText.text = $"{currCoins}";
        }
    }
    public void ToggleCoinsUI(bool isActive) {
        if (coinsText != null) {
            coinsText.gameObject.SetActive(isActive);
        }
    }
}
