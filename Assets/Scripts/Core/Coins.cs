using UnityEngine;

public class Coins : MonoBehaviour {
    public int currCoins = 0;
    
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
    public void RestoreCoins() {
        currCoins = PlayerPrefs.GetInt(SavedData.Coins);
    }
    public void AddCoins(int _coins) {
        currCoins += _coins;
        SaveCoinsData(currCoins);
    }
    public void RemoveCoins(int _coins) {
        currCoins -= _coins;
        SaveCoinsData(currCoins);
    }
    private void SaveCoinsData(int _coins) {
        PlayerPrefs.SetInt(SavedData.Coins, _coins);
        PlayerPrefs.Save();
    }
}
