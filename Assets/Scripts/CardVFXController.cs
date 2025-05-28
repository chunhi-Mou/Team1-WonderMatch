using DG.Tweening;
using UnityEngine;

public class CardVFXController : MonoBehaviour {
    [Range(0f, 6.3f)]
    public float fadeAmount = 6.3f;

    [Range(0f, 1f)]
    public float brightness = 1f;

    private SpriteRenderer spriteRenderer;
    private MaterialPropertyBlock block;

    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        block = new MaterialPropertyBlock();
    }

    void Start() {
        ApplyEffects();
    }

    public void ApplyEffects() {
        spriteRenderer.GetPropertyBlock(block);
        block.SetFloat("_do_tan_bien", fadeAmount);
        block.SetFloat("_brightness", brightness);
       // Color rainbowColor = Color.HSVToRGB(1f, 1f, 1f);
       // block.SetColor("_mau_vien", rainbowColor);
        spriteRenderer.SetPropertyBlock(block);
    }

    void OnValidate() {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        if (block == null)
            block = new MaterialPropertyBlock();

        ApplyEffects();
    }
    public void DimCard() {
        brightness = 0.35f;
        ApplyEffects();
    }
    public void BrightenCard() {
        brightness = 1.2f;
        ApplyEffects();
    }
    public void FadeOut(float duration = 1f, System.Action onComplete = null) {
        DOTween.To(() => fadeAmount, x => {
            fadeAmount = x;
            ApplyEffects();
        }, 0f, duration).OnComplete(()=> onComplete?.Invoke());
    }

}
