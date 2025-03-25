using UnityEngine;

public class CardInfo : MonoBehaviour {
    public CardDatabase cardDatabase;
    public CardData cardData;
    public SpriteRenderer spriteRenderer;
    public Collider cardCollider;
    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        cardCollider = GetComponent<Collider>();
    }
    public void SetCardData(CardType curType) {
        if (spriteRenderer == null) {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        CardData cardFromBase = cardDatabase.cards[(int)curType];
        cardData.sprite = cardFromBase.sprite;
        cardData.cardType = curType;
        spriteRenderer.sprite = cardData.sprite;
    }
}
