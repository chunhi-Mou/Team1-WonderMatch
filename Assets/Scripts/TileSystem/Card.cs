using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(BoxCollider))]
public class Card : MonoBehaviour {
    [SerializeField] CardDatabase cardDatabase;
    public CardData cardData;

    private SpriteRenderer spriteRenderer;
    bool isSelectable = true; //For Vision Only
    bool isMoving = false;
    private void OnMouseDown() {
        if (!isSelectable) return;
        Collider collider = GetComponent<Collider>();
        collider.enabled = false;
        GameEvents.OnFoundPosOfCard += MoveCardTo; //Nhi: đăng kí Event nhận Target
        GameEvents.OnCardSelectedInvoke(this);
    }
    private void GetCardData() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        CardData cardFromBase = cardDatabase.cards[(int)cardData.cardType];
        cardData.sprite = cardFromBase.sprite;
        spriteRenderer.sprite = cardData.sprite;
    }
    public void MoveCardTo(Transform target) {
        if(isMoving) return;
        isMoving = true;
        GameEvents.OnFoundPosOfCard -= MoveCardTo;//Nhi: huỷ đăng kí Event nhận Target
        gameObject.transform.DOMove(target.position, 0.5f).OnComplete(() => {
            GameEvents.OnCardDoneMovingInvoke();
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
