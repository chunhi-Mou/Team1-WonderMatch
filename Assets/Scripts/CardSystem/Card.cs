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

    private void Start() {
        //BẠN ART MUỐN LÀM BALATRO
    }

    private void Awake() {
        GameModeManager.instance.isPaused = false;
        GameModeManager.instance.isProcessingCard = false;
        GameModeManager.instance.isUsingPowers = false;
        cardOverlapChecker = GetComponent<CardOverlapChecker>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnMouseDown() {
        if (GameModeManager.instance.isPaused || GameModeManager.instance.isProcessingCard || GameModeManager.instance.isUsingPowers) return;
        AudioManager.instance.Play(SoundEffect.Pop);
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

        UpdateCardsState();
    }
    public void UpdateCardsState() {
        cardOverlapChecker.UpdateBelowTiles();
        GetComponent<Collider>().enabled = false;

        GameEvents.OnCardSelectedInvoke(this);
    }
    public void GetCardData() {
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
        SetSelectableData(true);
        gameObject.transform.DOMove(target, _duration)
            .SetEase(easeType)
            .OnComplete(() => {
                CardDoneMovingStatusUpdate();
            });
    }
    public void MoveCardToStack(Vector3 target, float _duration = 0.5f, Ease easeType = Ease.OutBack) {
        spriteRenderer.sortingOrder = 1000;
        SetSelectableData(true);

        float randomRotation = Random.Range(-5f, 5f);
        Vector3 originalScale = new Vector3(1f, 1f, 1f);
        Vector3 finalScale = originalScale * 0.15f;

        Sequence sequence = DOTween.Sequence();

        sequence.Append(transform.DOMove(target, _duration).SetEase(easeType));
        sequence.Join(transform.DOScale(originalScale * 0.18f, _duration * 0.7f));
        sequence.Append(transform.DOScale(finalScale, _duration * 0.3f));

        sequence.Join(transform.DORotate(new Vector3(0, 0, randomRotation), _duration));
        sequence.Append(transform.DOShakePosition(0.2f, 0.1f, 10, 90, false, true));

        sequence.Join(spriteRenderer.DOFade(0.8f, _duration * 0.5f));

        sequence.OnComplete(() => {
            spriteRenderer.DOFade(1f, 0.2f); 
            CardDoneMovingStatusUpdate();
        });
    }

    private void CardDoneMovingStatusUpdate() {
        cardOverlapChecker.NotifyTilesBelow();
        GameEvents.OnCardDoneMovingInvoke();
        spriteRenderer.sortingOrder = 0;
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
        transform.rotation = Quaternion.identity;
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
        spriteRenderer.DOColor(new Color(123f / 255f, 122f / 255f, 122f / 255f, 1f), 0.2f);
    }
    void SetWhiteSprite() {
        spriteRenderer.DOColor(Color.white, 0.2f);
    }
}
