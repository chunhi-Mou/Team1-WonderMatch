using UnityEngine;
using UnityEngine.UI;

public class WinLosePanel : MonoBehaviour {
    [SerializeField] GameObject WinUI;
    [SerializeField] GameObject LoseUI;
    [SerializeField] Button replayButton;
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
        if(HeartsSystem.hearts > 0) replayButton.interactable = true;
        else replayButton.interactable = false;
        LoseUI.SetActive(true);
        HeartsSystem.LoseHeart();
    }
    public void OnRevivePress() {
        Debug.Log("YOU WATCHED AD!!!");
        LoseUI.SetActive(false);
        GameModeManager.instance.ResumeGame();
        Card lastCard = CardHistory.Instance.UndoLastMove();
        if (lastCard != null) {
            AudioManager.instance.Play(SoundEffect.Undo);
            lastCard.UndoMove();
        }
    } 
}
