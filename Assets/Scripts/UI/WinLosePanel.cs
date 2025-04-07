using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WinLosePanel : MonoBehaviour {
    [Header("Win")]
    [SerializeField] CanvasGroup WinBG;
    [SerializeField] RectTransform WinContent;
    [SerializeField] RectTransform WinBanner;
    [SerializeField] TextMeshProUGUI WinCoinsTxt;
    [SerializeField] TextMeshProUGUI WinTimerTxt;

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
        AudioManager.instance.PauseAll();
        WinUI.SetActive(true);
        WinBG.gameObject.SetActive(true);
        WinContent.gameObject.SetActive(true);

        WinBG.alpha = 0f;
        WinBG.DOFade(1f, 0.3f).SetEase(Ease.InSine).SetUpdate(true);  // Quick fade-in for BG

        WinContent.localScale = Vector3.zero;
        WinBanner.localScale = Vector3.zero;

        AudioManager.instance.Play(SoundEffect.Win);
        WinContent.DOScale(1f, 0.5f).SetEase(Ease.OutBack).SetDelay(0.2f).SetUpdate(true);
        WinBanner.DOScale(1f, 0.5f).SetEase(Ease.OutBack).SetDelay(0.4f).SetUpdate(true);

        WinCoinsTxt.DOFade(1f, 0.5f).SetDelay(0.6f).SetUpdate(true);
        int seconds = Mathf.FloorToInt(TimerPanel.timeRemaining);
        WinTimerTxt.text = seconds.ToString() + "s";
    }


    private void ShowUpLoseUI() {
        if (HeartsSystem.hearts > 0) replayButton.interactable = true;
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
