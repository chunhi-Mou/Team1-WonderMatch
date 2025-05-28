using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class OutOfHeartPanel : MonoBehaviour
{
    public GameObject content;
    public Button panel;
    public Button refillButton;
    private void Start()
    {
        panel.onClick.RemoveAllListeners();
        refillButton.onClick.RemoveAllListeners();

        panel.onClick.AddListener(ReturnToMap);
        refillButton.onClick.AddListener(RefillHeart);

        content.SetActive(false);
        panel.gameObject.SetActive(false);
        refillButton.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        GameEvents.OnOutOfHeart += ShowPopUp;
    }
    private void OnDisable()
    {
        GameEvents.OnOutOfHeart -= ShowPopUp;
    }
    private void ShowPopUp()
    {
        content.transform.DOScale(Vector3.one * 1.25f, 0.3f).SetEase(Ease.OutBack);
        content.gameObject.SetActive(true);
        refillButton.gameObject.SetActive(true);
        panel.gameObject.SetActive(true);
    }
    private void ReturnToMap()
    {
        panel.gameObject.SetActive(false);
        content.gameObject.SetActive(true);
        content.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack)
            .SetUpdate(true)
            .OnComplete(() => {
            content.gameObject.SetActive(false);
        });
    }
    private void RefillHeart()
    {
        HeartsSystem.instance.WatchAdHearts();
        ReturnToMap();
    }
}
