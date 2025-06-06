﻿using UnityEngine;
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
        if (cardData.cardType != CardType.SCard_A && cardData.cardType != CardType.SCard_B && cardData.cardType != CardType.SCard_C && cardData.cardType != CardType.SCard_D) return;
        int SCardTaken = PlayerPrefs.GetInt("SCard" + (int)cardData.cardType, 0);
        if (SCardTaken == 1) {
            cardOverlapChecker.UpdateAboveTiles();
            cardOverlapChecker.UpdateBelowTiles();
            cardOverlapChecker.NotifyTilesBelow();
            state = CardState.inBoard;
            gameObject.SetActive(false);
        }
        
    }
    private void OnDisable() {
        GameEvents.OnDoneChooseCardType -= SetCardData;
    }
    private void Start() {
        cardOverlapChecker.UpdateAboveTiles();
        cardOverlapChecker.UpdateBelowTiles();
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
        if (cardData.cardType != CardType.SCard_A && cardData.cardType != CardType.SCard_B && cardData.cardType != CardType.SCard_C && cardData.cardType != CardType.SCard_D)
        {
            PushCardToStack();
        } else {
            GOTCollectableCard();
        }
    }
    public void GOTCollectableCard () {
        if (!isSelectable) return;
        PlayerPrefs.SetInt("SCard" + (int)cardData.cardType, 1);
        PlayerPrefs.Save();
        cardOverlapChecker.UpdateAboveTiles();
        cardOverlapChecker.UpdateBelowTiles();
        transform.DOMove(Vector3.zero, 0.45f).SetAutoKill(true).OnComplete(()=> cardOverlapChecker.NotifyTilesBelow());
        transform.DOScale(transform.localScale * 4.67f, 0.45f)
            .OnComplete(() => {
                DOVirtual.DelayedCall(0.6f, () => {
                    AudioManager.instance.Play(SoundEffect.specialCard);
                    cardOverlapChecker.NotifyTilesBelow();
                    cardVFXController.FadeOut(0.8f);
                    DOVirtual.DelayedCall(0.81f, () => {
                        state = CardState.inBoard;
                        Board.instance.UpdateCardsList();
                        Board.instance.CheckWinGame();
                        gameObject.SetActive(false);
                    });
                });
            }).SetAutoKill(true);
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
    public void MoveCardTo(Vector3 target, float _duration=0.5f, Ease easeType = Ease.Linear, System.Action onComplete=null) {
        SetPriority(-2);
        SetSelectableData(true);
        gameObject.transform.DOMove(target, _duration)
            .SetEase(easeType)
            .OnComplete(() => {
                SetPriority(0);
                CardDoneMovingStatusUpdate();
                onComplete?.Invoke();
            }).SetAutoKill(true);
    }
    public void MoveCardToStack(Vector3 target, float _duration = 0.5f, Ease easeType = Ease.OutBack) {
        SetPriority(-2);
        SetSelectableData(true);

        float randomRotation = Random.Range(-5f, 5f);
        Vector3 originalScale = new Vector3(1f, 1f, 1f);
        Vector3 finalScale = originalScale * 0.15f;
        DOVirtual.DelayedCall(0.1f, () => {
            //cardOverlapChecker.NotifyTilesBelow();
        });

        Sequence sequence = DOTween.Sequence();

        sequence.Append(transform.DOMove(target, _duration * 0.7f).SetEase(easeType));
        sequence.Join(transform.DOScale(originalScale * 0.18f, _duration * 0.7f));
        sequence.Join(transform.DORotate(new Vector3(0, 0, randomRotation), _duration * 0.7f));
        sequence.Join(spriteRenderer.DOFade(0.8f, _duration * 0.5f));

        sequence.Append(transform.DOScale(finalScale, _duration * 0.3f));
        sequence.Join(spriteRenderer.DOFade(1f, _duration * 0.3f));

        sequence.Append(transform.DOShakePosition(0.1f, 0.05f, 5, 90, false, true));

        sequence.OnComplete(() => {
            GameModeManager.instance.isMovingCardsInStack = false;
            spriteRenderer.DOFade(1f, 0.2f);
            SetPriority(0);
            CardDoneMovingStatusUpdate();
        });
        sequence.SetAutoKill(true);
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
        transform.DOKill();
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
    public void OverLapChecker() {
        cardOverlapChecker.NotifyTilesBelow();
    }
    private void OnDestroy() {
        transform.DOKill();
    }
}
