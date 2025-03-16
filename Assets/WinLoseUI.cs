using UnityEngine;

public class WinLoseUI : MonoBehaviour {
    [SerializeField] GameObject WinUI;
    [SerializeField] GameObject LoseUI;
    private void OnEnable() {
        GameEvents.OnWinGame += ShowUpWinUI;
        GameEvents.OnLoseGame += ShowUpLoseUI;
    }
    private void OnDisable() {
        GameEvents.OnWinGame -= ShowUpWinUI;
        GameEvents.OnLoseGame -= ShowUpLoseUI;
    }
    private void ShowUpWinUI() {
        WinUI.SetActive(true);
    }
    private void ShowUpLoseUI() {
        LoseUI.SetActive(true);
    }
}
