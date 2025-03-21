using TMPro;
using UnityEngine;

public class CoinsUI : MonoBehaviour {
    [SerializeField] RectTransform spendCoinsUI;
    [SerializeField] RectTransform xButton;
    [SerializeField] RectTransform adButton;
    [SerializeField] RectTransform spendButton;
    [SerializeField] RectTransform[] powerImages;
    [SerializeField] TextMeshProUGUI coinTxt;
    private void OnEnable() {
        GameEvents.OnSpendCoinsNeeded += TurnOnSpendCoinsUI;
    }
    private void OnDisable() {
        GameEvents.OnSpendCoinsNeeded -= TurnOnSpendCoinsUI;
    }
    private PowerType currPowerType;
    public void TurnOnSpendCoinsUI(PowerType powerType) {
        GameModeManager.instance.TurnOnUIAndPauseGame();
        coinTxt.text = CoinsManager.Instance.currCoins.ToString();
        spendCoinsUI.gameObject.SetActive(true);
        this.currPowerType = powerType;

        foreach (var img in powerImages) {
            img.gameObject.SetActive(false);
        }
        powerImages[(int)powerType].gameObject.SetActive(true);

        if(CoinsManager.Instance.CanBuyPower()) {
            spendButton.gameObject.SetActive(true);
            adButton.gameObject.SetActive(false);
        } else {
            spendButton.gameObject.SetActive(false);
            adButton.gameObject.SetActive(true);
        }
    }
    public void OnSpendButtonPress() {
        if (CoinsManager.Instance.CanBuyPower()) {
            CoinsManager.Instance.TrySpendCoins();
            PowerUpsManager.Instance.ResetPower(currPowerType);
            spendCoinsUI.gameObject.SetActive(false);
            GameModeManager.instance.TurnOffUIAndResumeGame();
        }
    }
    public void OnXPress() {
        spendCoinsUI.gameObject.SetActive(false);
        GameModeManager.instance.TurnOffUIAndResumeGame();
    }
    public void OnAdButtonPress() {
        PowerUpsManager.Instance.ResetPower(currPowerType);
        Debug.Log("YOU WATCHED AD");
        spendCoinsUI.gameObject.SetActive(false);
        GameModeManager.instance.TurnOffUIAndResumeGame();
    }
}
