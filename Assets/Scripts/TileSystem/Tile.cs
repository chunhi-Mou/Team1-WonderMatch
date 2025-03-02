using UnityEngine;
using DG.Tweening;

public class Tile : MonoBehaviour {
    [SerializeField] CardDatabase cardDatabase;
    public CardData card;

    private SpriteRenderer spriteRenderer;
    bool isSelectable = true;
    bool isMoving = false;
    private void OnMouseDown() {
        if(!isSelectable) return;
        isSelectable = false;
        GameEvents.OnTileSelectedInvoke(this);
    }
    private void GetCardData() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        card = cardDatabase.cards[(int)card.cardType];
        spriteRenderer.sprite = card.sprite;
    }
    public void MoveTileTo(Transform target) {
        if(isMoving) return;
        isMoving = true;
        gameObject.transform.DOMove(target.position, 1f).OnComplete(() => {
            GameEvents.OnTileDoneMovingInvoke();
            isSelectable = true;
            isMoving=false;
        });
    }
    private void OnValidate() {
        GetCardData();
    }
}
