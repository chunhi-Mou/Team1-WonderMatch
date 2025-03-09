using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(BoxCollider))]
public class Card : MonoBehaviour {
    [SerializeField] CardDatabase cardDatabase;
    public CardData cardData;
    public CardState state = CardState.inBoard;

    private Vector3 prevPosition;
    private CardOverlapChecker cardOverlapChecker;
    private SpriteRenderer spriteRenderer;

    private bool isSelectable = true; //For Vision Only
    private bool isMoving = false;
    
    private void Awake() {
        cardOverlapChecker = GetComponent<CardOverlapChecker>();
    }
    private void OnMouseDown() {
        PushCardToStack();
    }
    public void PushCardToStack() {
        if (!isSelectable) return;
        if (state == CardState.inBoard) {
            prevPosition = transform.position;
            state = CardState.inStack;
        } else {
            state = CardState.inBoard;
        }

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
    public void MoveCardTo(Vector3 target, float _duration=0.5f, Ease easeType = Ease.Linear) {
        if(isMoving) return;
        isMoving = true;
        GameEvents.OnFoundPosOfCard -= MoveCardTo;//Nhi: huỷ đăng kí Event nhận Target
        gameObject.transform.DOMove(target, _duration)
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
    public void UndoMove() {
        if (isMoving && state != CardState.inStack) return;
        MoveCardTo(prevPosition, 0.5f, Ease.OutQuad);
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
