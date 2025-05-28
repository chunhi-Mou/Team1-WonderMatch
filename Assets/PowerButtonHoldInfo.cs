using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using DG.Tweening;

public class PowerButtonHoldInfo : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler {
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private float holdDuration = 0.0001f;
    [SerializeField] private float fadeDuration = 0.0001f;
    [SerializeField] private Vector3 showScale = Vector3.one;
    [SerializeField] private Vector3 hideScale = Vector3.zero;

    private Coroutine holdCoroutine;
    private CanvasGroup canvasGroup;

    private void Awake() {
        if (infoPanel != null) {
            canvasGroup = infoPanel.GetComponent<CanvasGroup>();
            if (canvasGroup == null) {
                canvasGroup = infoPanel.AddComponent<CanvasGroup>();
            }

            infoPanel.transform.localScale = hideScale;
            canvasGroup.alpha = 0;
            infoPanel.SetActive(false);
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        holdCoroutine = StartCoroutine(HoldTimer());
    }

    public void OnPointerUp(PointerEventData eventData) {
        CancelHold();
    }

    public void OnPointerExit(PointerEventData eventData) {
        CancelHold();
    }

     public bool IsHolding { get; private set; } = false;
    public bool ShowedInfo { get; private set; } = false;

    private IEnumerator HoldTimer()
    {
        IsHolding = true;
        yield return new WaitForSeconds(holdDuration);
        ShowInfo();
    }

    private void CancelHold()
    {
        IsHolding = false;
        if (holdCoroutine != null)
        {
            StopCoroutine(holdCoroutine);
            holdCoroutine = null;
        }

        HideInfo();
    }

    private void ShowInfo() {
        if (infoPanel == null) return;
        ShowedInfo = true;
        infoPanel.SetActive(true);
        infoPanel.transform.localScale = hideScale;
        canvasGroup.alpha = 0;
        infoPanel.transform.DOScale(showScale, fadeDuration).SetEase(Ease.OutBack);
        canvasGroup.DOFade(1f, fadeDuration);
    }

    private void HideInfo() {
        if (infoPanel == null || !infoPanel.activeSelf) return;
        infoPanel.transform.DOScale(hideScale, fadeDuration).SetEase(Ease.InBack);
        canvasGroup.DOFade(0f, fadeDuration).OnComplete(() => {
            ShowedInfo = false;
            infoPanel.SetActive(false);
        });
    }
}
