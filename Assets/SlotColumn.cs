using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

public class SlotColumn : MonoBehaviour {
    public RectTransform container;             
    public List<Image> slotImages;       
    public float symbolHeight = 83f;
    public float spinSpeed = 0.1f;

    private Sprite[] symbolSprites;
    private RectTransform[] visibleSlots;
    private bool spinning = false;
    private Sprite forcedSymbol;

    private System.Action onStop;

    private int spinCounter = 0;
    private int maxSpins = 20;

    public void Init(Sprite[] allSymbols, RectTransform[] showSlots) {
        symbolSprites = allSymbols;
        visibleSlots = showSlots;
    }

    public void StartSpin() {
        spinning = true;
        spinCounter = 0;
        SpinStep();
    }

    public void StopSpin(Sprite matchSymbol, System.Action callback) {
        spinning = false;
        forcedSymbol = matchSymbol;
        onStop = callback;
    }

    private void SpinStep() {
        container.DOAnchorPosY(container.anchoredPosition.y + symbolHeight, spinSpeed)
            .SetEase(Ease.Linear)
            .SetUpdate(true) 
            .OnComplete(() => {
                var first = slotImages[0];
                slotImages.RemoveAt(0);
                slotImages.Add(first);
                first.SetNativeSize();
                first.rectTransform.SetAsLastSibling();

                first.sprite = spinning ? symbolSprites[Random.Range(0, symbolSprites.Length)] : forcedSymbol;

                container.anchoredPosition = new Vector2(container.anchoredPosition.x, container.anchoredPosition.y - symbolHeight);

                spinCounter++;

                if (spinning || (!spinning && spinCounter < maxSpins)) {
                    SpinStep();
                } else {
                    foreach (var img in slotImages) {
                        img.sprite = forcedSymbol;
                        img.SetNativeSize();
                    }
                    onStop?.Invoke();
                }
            });
    }

}
