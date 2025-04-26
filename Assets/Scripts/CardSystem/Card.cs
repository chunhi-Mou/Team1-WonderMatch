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
    public CardVFXController cardVFXController;

    private bool isSelectable = true; //For Vision Only

    private void OnEnable() {
        GameEvents.OnDoneChooseCardType += SetCardData;
    }
    private void OnDisable() {
        GameEvents.OnDoneChooseCardType -= SetCardData;
    }
    void SetCardData() {
        switch (SlotController.IdxCardType) {
            case 0:
                cardDatabase = Resources.Load<CardDatabase>("CardData/BICH_CardData");
                break;
            case 1:
                cardDatabase = Resources.Load<CardDatabase>("CardData/CO_CardData");
                break;
            case 2:
                cardDatabase = Resources.Load<CardDatabase>("CardData/RO_CardData");
                break;
            case 3:
                cardDatabase = Resources.Load<CardDatabase>("CardData/TEP_CardData");
                break;
        }
        GetCardData();
        Debug.Log("hi");
    }
    private void Awake() {
        GameModeManager.instance.isPaused = false;
        GameModeManager.instance.isProcessingCard = false;
        GameModeManager.instance.isUsingPowers = false;
        cardOverlapChecker = GetComponent<CardOverlapChecker>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        cardVFXController = GetComponent<CardVFXController>();
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
        SetPriority(-2);
        SetSelectableData(true);
        gameObject.transform.DOMove(target, _duration)
            .SetEase(easeType)
            .OnComplete(() => {
                SetPriority(0);
                CardDoneMovingStatusUpdate();
            });
    }
    public void MoveCardToStack(Vector3 target, float _duration = 0.5f, Ease easeType = Ease.OutBack) {
        SetPriority(-2);
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
            SetPriority(0);
            CardDoneMovingStatusUpdate();
        });
    }

    private void CardDoneMovingStatusUpdate() {
        cardOverlapChecker.NotifyTilesBelow();
        GameEvents.OnCardDoneMovingInvoke();
        SetPriority(0);
    }
    public void SetSelectableData(bool _data) {
        this.isSelectable = _data;
        if(!this.isSelectable) {
            cardVFXController.DimCard();
        } else {
            cardVFXController.BrightenCard();
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
        SetPriority(-2);
        CardAnimation.PlayCardShakeThenMove(this.transform, prevPosition, 0.3f, 0.5f, () => {
            HandleCardMoveComplete();
        });
        state = CardState.inBoard;
        this.isSelectable = true;
    }
    private void HandleCardMoveComplete() {
        cardOverlapChecker.NotifyTilesBelow();
        GameEvents.OnCardDoneMovingInvoke();
        SetPriority(0);
    }
    private void SetPriority(float _val) {
    }
}
