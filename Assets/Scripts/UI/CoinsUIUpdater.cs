using TMPro;
using UnityEngine;

public class CoinsUIUpdater : MonoBehaviour {
    [SerializeField] TextMeshProUGUI coinUITxt;
    private void Update() {
        if(coinUITxt.text != CoinsManager.Instance.currCoins.ToString())
            coinUITxt.text = CoinsManager.Instance.currCoins.ToString();
    } 
}
