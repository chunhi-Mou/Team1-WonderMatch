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
    [SerializeField] Image timerIcon;
    [SerializeField] Image coinIcon;
    [SerializeField] WinEffectController winEffectController;

    [Header("Lose")]
    [SerializeField] GameObject LoseBG;
    [SerializeField] GameObject DefeatPanel;

    [SerializeField] GameObject WinUI;
    [SerializeField] GameObject LoseUI;
    [SerializeField] Button replayButton;

    private void OnEnable() {
        winEffectController = GetComponent<WinEffectController>();
        GameEvents.OnWinGame += ShowUpWinUI;
        GameEvents.OnLoseGame += ShowUpLoseUI;
    }

    private void OnDisable() {
        GameEvents.OnWinGame -= ShowUpWinUI;
        GameEvents.OnLoseGame -= ShowUpLoseUI;
    }

    private void ShowUpWinUI() {
        AudioManager.instance.PauseAll();
        WinBG.gameObject.SetActive(true);
        WinContent.gameObject.SetActive(true);

        WinBG.alpha = 0f;
        WinBG.DOFade(1f, 0.3f).SetEase(Ease.InSine).SetUpdate(true);

        Vector3 startPos = WinContent.localPosition + new Vector3(0, 3000f, 0);
        WinContent.localPosition = startPos;

        WinBanner.localScale = Vector3.zero;
        AudioManager.instance.Play(SoundEffect.Win);

        WinContent.DOLocalMoveY(0f, 0.7f).SetEase(Ease.InSine).SetDelay(0.2f).OnComplete(() => {
            WinContent.DOShakePosition(0.3f, strength: new Vector3(0, 10f, 0), vibrato: 15, randomness: 90, snapping: false, fadeOut: true).SetUpdate(true);
            AudioManager.instance.Play(SoundEffect.ImpactHeavy);
            winEffectController.PlayEffect();
            DOVirtual.DelayedCall(0.4f, () => {
                WinUI.SetActive(true);
                WinBanner.DOScale(1f, 0.1f).SetEase(Ease.OutBack).SetUpdate(true);

                WinCoinsTxt.DOFade(1f, 0.5f).SetDelay(0.6f).SetUpdate(true);

                int seconds = Mathf.FloorToInt(TimerPanel.timeRemaining);
                DOVirtual.Int(0, seconds, 1f, value =>
                {
                    WinTimerTxt.text = value.ToString() + "s";
                }).SetDelay(0.7f).SetEase(Ease.Linear).SetUpdate(true);

                SpawnAndAnimateCoins();
            });
        }).SetUpdate(true);
    }


    private void ShowUpLoseUI() {
        if (HeartsSystem.hearts > 0) replayButton.interactable = true;
        else replayButton.interactable = false;
        LoseUI.SetActive(true);
        LoseBG.SetActive(true);
        DefeatPanel.transform.localScale = Vector3.zero;
        DefeatPanel.transform.DOScale(2f, 0.5f).SetEase(Ease.OutBack).SetDelay(0.2f).SetUpdate(true);
        HeartsSystem.LoseHeart();
    }

    public void OnRevivePress() {
        Debug.Log("YOU WATCHED AD!!!");
        LoseUI.SetActive(false);
        TimerPanel.ResetTime();
        GameModeManager.instance.ResumeGame();
        UndoThreeCard();
    }
    private void UndoThreeCard()
    {
        for (int i=0; i<3; i++)
        {
            Card lastCard = CardHistory.Instance.UndoLastMove();
            if (lastCard != null) {
                AudioManager.instance.Play(SoundEffect.Undo);
                lastCard.UndoMove();
            }
        }
    }
    void SpawnAndAnimateCoins() {

    }

}
