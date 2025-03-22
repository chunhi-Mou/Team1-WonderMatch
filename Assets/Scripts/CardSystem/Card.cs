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
    
    private void Awake() {
        cardOverlapChecker = GetComponent<CardOverlapChecker>();
    }
    private void OnMouseDown() {
        if (GameModeManager.instance.isPaused || GameModeManager.instance.isProcessingCard) return;
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
        GetComponent<Collider>().enabled = false;

        GameEvents.OnCardSelectedInvoke(this);
    }
    public void GetCardData() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        CardData cardFromBase = cardDatabase.cards[(int)cardData.cardType];
        cardData.sprite = cardFromBase.sprite;
        spriteRenderer.sprite = cardData.sprite;
    }
    public void SetCardData(int curType) {
        spriteRenderer = GetComponent<SpriteRenderer>();
        CardData cardFromBase = cardDatabase.cards[curType];
        cardData.sprite = cardFromBase.sprite;
        cardData.cardType = (CardType) curType;
        spriteRenderer.sprite = cardData.sprite;
    }
    public void MoveCardTo(Vector3 target, float _duration=0.5f, Ease easeType = Ease.Linear) {
        spriteRenderer.sortingOrder = 1000;
        gameObject.transform.DOMove(target, _duration)
            .SetEase(easeType)
            .OnComplete(() => {
                cardOverlapChecker.NotifyTilesBelow();
                GameEvents.OnCardDoneMovingInvoke();
                spriteRenderer.sortingOrder = 0;
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
        if (!GameModeManager.instance.isProcessingCard && state != CardState.inStack) return;
        GameEvents.OnUndoPressedInvoke(this);
        GetComponent<Collider>().enabled = true;
        CardAnimation.PlayCardShakeThenMove(this.transform, prevPosition, 0.3f, 0.5f, () => {
            HandleCardMoveComplete();
        });
        state = CardState.inBoard;
        this.isSelectable = true;
    }
    private void HandleCardMoveComplete() {
        cardOverlapChecker.NotifyTilesBelow();
        GameEvents.OnCardDoneMovingInvoke();
        spriteRenderer.sortingOrder = 0;
    }
    void DarkenSprite() {
        spriteRenderer.DOColor(new Color(123f / 255f, 122f / 255f, 122f / 255f, 1f), 0.15f);
    }
    void SetWhiteSprite() {
        spriteRenderer.DOColor(Color.white, 0.15f);
    }
}
