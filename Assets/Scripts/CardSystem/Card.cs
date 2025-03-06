using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(BoxCollider))]
public class Card : MonoBehaviour {
    [SerializeField] CardDatabase cardDatabase;
    public CardData cardData;

    private CardOverlapChecker cardOverlapChecker;
    private SpriteRenderer spriteRenderer;
    private bool isSelectable = true; //For Vision Only
    private bool isMoving = false;
    private void Awake() {
        cardOverlapChecker = GetComponent<CardOverlapChecker>();
    }
    private void OnMouseDown() {
        if (!isSelectable) return;
        cardOverlapChecker.UpdateBelowTiles();
        Collider collider = GetComponent<Collider>();
        collider.enabled = false;
        GameEvents.OnFoundPosOfCard += MoveCardTo; //Nhi: đăng kí Event nhận Target
        GameEvents.OnCardSelectedInvoke(this);
    }
    public void GetCardData() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        CardData cardFromBase = cardDatabase.cards[(int)cardData.cardType];
        cardData.sprite = cardFromBase.sprite;
        spriteRenderer.sprite = cardData.sprite;
    }
    public void MoveCardTo(Transform target, float _duration, Ease easeType = Ease.Linear) {
        if(isMoving) return;
        isMoving = true;
        GameEvents.OnFoundPosOfCard -= MoveCardTo;//Nhi: huỷ đăng kí Event nhận Target
        gameObject.transform.DOMove(target.position, _duration)
            .SetEase(easeType)
            .OnComplete(() => {
                cardOverlapChecker.NotifyTilesBelow();
                GameEvents.OnCardDoneMovingInvoke();
                this.isSelectable = true;
                isMoving = false;
        });
    }
    public void SetSelectableData(bool _data) {
        this.isSelectable = _data;
        if(!this.isSelectable) {
            this.DarkenSprite();
        } else {
            this.SetWhiteSprite();
        }
    }
    public void DisableMatchedCard(Card card) {
        card.gameObject.SetActive(false); 
    }
    void DarkenSprite() {
        spriteRenderer.DOColor(new Color(123f / 255f, 122f / 255f, 122f / 255f, 1f), 0.5f);
    }
    void SetWhiteSprite() {
        spriteRenderer.DOColor(Color.white, 0.5f);
    }
    private void OnValidate() {
        GetCardData();
    }
}
