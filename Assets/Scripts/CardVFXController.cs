using UnityEngine;

public class CardVFXController : MonoBehaviour {
    [Range(0f, 6.3f)]
    public float fadeAmount = 6.3f;

    private SpriteRenderer spriteRenderer;
    private MaterialPropertyBlock block;

    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        block = new MaterialPropertyBlock();
    }

    void Start() {
        ApplyFade();
    }

    public void ApplyFade() {
        spriteRenderer.GetPropertyBlock(block);
        block.SetFloat("_do_tan_bien", fadeAmount);
        //block.SetColor("")
        Color rainbowColor = Color.HSVToRGB(fadeAmount / 6.3f, 1f, 1f);
        block.SetColor("_mau_vien", rainbowColor);
        spriteRenderer.SetPropertyBlock(block);
    }

    void OnValidate() {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        if (block == null)
            block = new MaterialPropertyBlock();

        ApplyFade();
    }
}
