using UnityEngine;

public class CoinsManager : MonoBehaviour {
    public int currCoins { get; private set; }
    public static int powerCost = 100;

    #region Singleton
    public static CoinsManager Instance { get; private set; }
    private void Awake() {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    #endregion
    private void Start() {
        this.RestoreCoins();
    }
    private void OnEnable() {
        GameEvents.OnWinGame += AddCoinsOnWin;
        GameEvents.OnSpendCoins += TrySpendCoins;
    }
    private void OnDisable() {
        GameEvents.OnWinGame -= AddCoinsOnWin;
        GameEvents.OnSpendCoins -= TrySpendCoins;
    }
    public void AddCoinsOnWin() {
        this.AddCoins(20);
    }
    private void RestoreCoins() {
        currCoins = PlayerPrefs.GetInt(SavedData.Coins, 0);
        GameEvents.OnCoinsChangedInvoke(currCoins);
    }
    public void AddCoins(int _coins) {
        currCoins += _coins;
        SaveCoinsData(currCoins);
    }
    public bool TrySpendCoins(int _coins) {
        if (currCoins < _coins) return false;
        currCoins -= _coins;
        SaveCoinsData(currCoins);
        return true;
    }
    private void SaveCoinsData(int _coins) {
        PlayerPrefs.SetInt(SavedData.Coins, _coins);
        PlayerPrefs.Save();
        GameEvents.OnCoinsChangedInvoke(_coins);
    }
    public bool CanBuyPower() {
        return currCoins >= powerCost;
    }
}
