using UnityEngine;
using DG.Tweening;

public class Tile : MonoBehaviour {
    [SerializeField] CardDatabase cardDatabase;
    public CardData card;

    private SpriteRenderer spriteRenderer;
    bool isSelectable = true;
    private void OnDisable() {
        GameEvents.OnFoundPosOfTile -= MoveTileTo;
    }
    private void OnMouseDown() {
        if(!isSelectable) return;
        Collider2D collider2D = GetComponent<Collider2D>();
        collider2D.enabled = false;
        GameEvents.OnFoundPosOfTile += MoveTileTo;

        GameEvents.OnTileSelectedInvoke(this);
    }
    private void GetCardData() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        card = cardDatabase.cards[(int)card.cardType];
        spriteRenderer.sprite = card.sprite;
    }
    public void MoveTileTo(Transform target) {
        gameObject.transform.DOMove(target.position, 2f).OnComplete(() => {
            GameEvents.OnTileDoneMovingInvoke();
            GameEvents.OnFoundPosOfTile -= MoveTileTo;
        });
    }
    private void OnValidate() {
        GetCardData();
    }
}
