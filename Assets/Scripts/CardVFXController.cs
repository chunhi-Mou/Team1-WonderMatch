using UnityEngine;

public class CardVFXController : MonoBehaviour {
    [Header("Card Appearance")]
    public Texture cardTexture;
    public float fade = 6.25f;
    public float alphaClip = 0.11f;
    public Color borderColor = Color.magenta;

    private SpriteRenderer spriteRenderer;
    private MaterialPropertyBlock block;

    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        block = new MaterialPropertyBlock();
    }

    void Start() {
        ApplyProperties();
    }

    public void ApplyProperties() {
        block.Clear();

        block.SetTexture("_MainTex", cardTexture);
        block.SetFloat("_do_tan_bien", fade);
        block.SetFloat("_alpha_clip", alphaClip);
        block.SetColor("_mau_vien", borderColor);

        spriteRenderer.SetPropertyBlock(block);
    }
}
