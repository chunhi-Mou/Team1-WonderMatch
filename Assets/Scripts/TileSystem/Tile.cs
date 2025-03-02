using UnityEngine;

public class Tile : MonoBehaviour {
    [SerializeField] CardDatabase cardDatabase;
    [SerializeField] CardData card;

    private SpriteRenderer spriteRenderer;
    private void GetCardData() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        card = cardDatabase.cards[(int)card.cardType];
        spriteRenderer.sprite = card.sprite;
    }
    private void OnValidate() {
        GetCardData();
    }
}
