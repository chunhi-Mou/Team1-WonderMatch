using UnityEngine;
using DG.Tweening;

public class Tile : MonoBehaviour {
    [SerializeField] CardDatabase cardDatabase;
    public CardData card;

    private SpriteRenderer spriteRenderer;
    bool isSelectable = true; //For Vision Only
    bool isMoving = false;
    private void OnMouseDown() {
        if (!isSelectable) return;
        Collider2D collider2D = GetComponent<Collider2D>();
        collider2D.enabled = false;
        GameEvents.OnFoundPosOfTile += MoveTileTo; //Nhi: đăng kí Event nhận Target
        GameEvents.OnTileSelectedInvoke(this);
    }
    private void GetCardData() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        CardData cardFromBase = cardDatabase.cards[(int)card.cardType];
        card.sprite = cardFromBase.sprite;
        spriteRenderer.sprite = card.sprite;
    }
    public void MoveTileTo(Transform target) {
        if(isMoving) return;
        isMoving = true;
        GameEvents.OnFoundPosOfTile -= MoveTileTo;//Nhi: huỷ đăng kí Event nhận Target
        gameObject.transform.DOMove(target.position, 0.5f).OnComplete(() => {
            GameEvents.OnTileDoneMovingInvoke();
            this.isSelectable = true;
            isMoving=false;
        });
    }
    public void SetSelectableData(bool _data) {
        this.isSelectable = _data;
    }
    private void OnValidate() {
        GetCardData();
    }
}
