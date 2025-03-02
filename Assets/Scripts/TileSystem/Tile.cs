using UnityEngine;
using DG.Tweening;

public class Tile : MonoBehaviour {
    [SerializeField] CardDatabase cardDatabase;
    [SerializeField] CardData card;

    private SpriteRenderer spriteRenderer;
    bool isSelectable = true;
    private void OnEnable() {
        GameEvents.OnFoundPosOfTile += MoveTileTo;
    }

    private void OnDisable() {
        GameEvents.OnFoundPosOfTile -= MoveTileTo;
    }
    private void OnMouseDown() {
        if(!isSelectable) return;
        GameEvents.OnTileSelectedInvoke();
    }
    private void GetCardData() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        card = cardDatabase.cards[(int)card.cardType];
        spriteRenderer.sprite = card.sprite;
    }
    public void MoveTileTo(Transform target) {
        gameObject.transform.DOMove(target.position, 2f);
    }
    private void OnValidate() {
        GetCardData();
    }
}
