using UnityEngine;
using DG.Tweening; 

public class SlotMachineMove : MonoBehaviour {
    Vector3 originalPos;
    int originalSiblingIndex;
    public RectTransform centerPos;
    public CanvasGroup slotBG;

    void Start() {
        originalPos = transform.position;
        originalSiblingIndex = transform.GetSiblingIndex();
    }

    public void SetToCenter() {
        transform.parent.SetAsLastSibling();
        Time.timeScale = 0f;
        slotBG.DOFade(1f, 0.5f).SetUpdate(true);
        transform.DOScale(new Vector3(2.5f, 2.5f, 2.5f), 0.2f).SetUpdate(true);
        transform.DOMove(centerPos.position, 0.5f).SetEase(Ease.InOutSine).SetUpdate(true);
    }

    public void SetToOriginalPos() {
        slotBG.DOFade(0f, 0.5f).SetUpdate(true).OnComplete(()=> slotBG.gameObject.SetActive(false));
        Time.timeScale = 1f;
        transform.DOScale(new Vector3(1.6f, 1.6f, 1.6f), 0.2f).SetUpdate(true);
        transform.DOMove(originalPos, 0.5f).SetEase(Ease.InOutSine).SetUpdate(true);
        transform.parent.SetSiblingIndex(originalSiblingIndex);
    }
}
