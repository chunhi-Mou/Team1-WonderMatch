using UnityEngine;
using DG.Tweening;

public class Tile : MonoBehaviour {
    [SerializeField] CardDatabase cardDatabase;
    public CardData card;

    private SpriteRenderer spriteRenderer;
    bool isSelectable = true;
    bool isMoving = false;
    private void OnDisable() {
        GameEvents.OnFoundPosOfTile -= MoveTileTo;
    }
    private void OnMouseDown() {
        if(!isSelectable) return;
        isSelectable = false;
        isMoving = true;
        GameEvents.OnFoundPosOfTile += MoveTileTo;
        GameEvents.OnTileSelectedInvoke(this);
    }
    private void GetCardData() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        card = cardDatabase.cards[(int)card.cardType];
        spriteRenderer.sprite = card.sprite;
    }
    public void MoveTileTo(Transform target) {
        if(isMoving) return;
        GameEvents.OnFoundPosOfTile -= MoveTileTo;
        gameObject.transform.DOMove(target.position, 2f).OnComplete(() => {
            GameEvents.OnTileDoneMovingInvoke();
            isSelectable = true;
        });
    }
    private void OnValidate() {
        GetCardData();
    }
}
