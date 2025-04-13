using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class StackLogic : MonoBehaviour {
    [SerializeField] private StackAnimation stackAnimation;
    [SerializeField] private Transform[] centerPos;
    [SerializeField] private List<Card> cardsInStack = new List<Card>();

    private Dictionary<CardType, List<Card>> cardTypeDictionary = new Dictionary<CardType, List<Card>>();
    private Queue<Card> pendingCards = new Queue<Card>();
    private bool isArranging = false;
    private bool isAddingCard = false;
    public int maxSizeStack = 8;
    public int currentSizeStack;
    private void Start()
    {
        currentSizeStack = 7;
    }
    private void OnEnable() {
        GameEvents.OnUndoPressed += RemoveUndoCard;
        GameEvents.OnMatchCards += ArrangeCards;
        GameEvents.OnCardSelected += GetCardTargetPos;
        GameEvents.OnCardDoneMoving += CheckMatch;
    }

    private void OnDisable() {
        GameEvents.OnUndoPressed -= RemoveUndoCard;
        GameEvents.OnMatchCards -= ArrangeCards;
        GameEvents.OnCardSelected -= GetCardTargetPos;
        GameEvents.OnCardDoneMoving -= CheckMatch;
    }


    private void GetCardTargetPos(Card card) {
        if (!card) return;

        if (isArranging || isAddingCard) {
            pendingCards.Enqueue(card);
            return;
        }

        int targetIndex = cardsInStack.Count;
        if (cardTypeDictionary.TryGetValue(card.cardData.cardType, out var sameTypeCards) && sameTypeCards.Count > 0) {
            targetIndex = cardsInStack.IndexOf(sameTypeCards[^1]) + 1;
        }

        AddCardToStack(targetIndex, card);
    }

    private void CheckFullStack() {
        if (cardsInStack.Count >= currentSizeStack) {
            Sequence seq = DOTween.Sequence();
            seq.Append(transform.DOShakePosition(1f, 0.15f, 10, 90));
            seq.Join(transform.DOShakeScale(0.8f, 0.1f));
            seq.SetEase(Ease.InOutSine)
               .SetUpdate(true)
               .OnComplete(() => {
                   GameEvents.OnLoseGameInvoke();
               });
        }
    }

    private void CheckMatch() {
        if (isAddingCard || isArranging || cardsInStack.Count < 3) return;

        for (int i = 0; i <= cardsInStack.Count - 3; i++) {
            var cardType = cardsInStack[i].cardData.cardType;
            if (cardType == cardsInStack[i + 1].cardData.cardType && cardType == cardsInStack[i + 2].cardData.cardType) {
                RemoveMatchFromStack(i);
                return;
            }
        }
        CheckFullStack();
    }

    private void AddCardToStack(int targetIndex, Card card) {
        isAddingCard = true;

        cardsInStack.Insert(targetIndex, card);
        cardTypeDictionary.TryAdd(card.cardData.cardType, new List<Card>());
        cardTypeDictionary[card.cardData.cardType].Add(card);

        stackAnimation.AnimateAddCard(cardsInStack, centerPos, targetIndex, () => {
            isAddingCard = false;
            CheckMatch();
            ProcessPendingCards();
        });
    }

    private void RemoveMatchFromStack(int currentMatchPos) {
        GameEvents.OnMatchCardsInvoke();
        List<Card> matchedCards = cardsInStack.GetRange(currentMatchPos, 3);
        isArranging = true;

        stackAnimation.AnimateRemoveMatch(matchedCards, () => {
            foreach (var card in matchedCards) {
                cardsInStack.Remove(card);
                cardTypeDictionary[card.cardData.cardType]?.Remove(card);
                card.DisableMatchedCard(card);
            }

            isArranging = false;
            ArrangeCards();
            ProcessPendingCards();
            GameEvents.OnMatchTilesDoneInvoke();
        });
    }

    private void ArrangeCards() {
        stackAnimation.AnimateArrangeCards(cardsInStack, centerPos);
    }

    private void ProcessPendingCards() {
        while (pendingCards.Count > 0 && !isAddingCard) {
            Card card = pendingCards.Dequeue();
            int targetIndex = cardsInStack.Count;
            if (cardTypeDictionary.TryGetValue(card.cardData.cardType, out var sameTypeCards) && sameTypeCards.Count > 0) {
                targetIndex = cardsInStack.IndexOf(sameTypeCards[^1]) + 1;
            }

            AddCardToStack(targetIndex, card);
        }
    }

    public void RemoveUndoCard(Card card) {
        cardsInStack.Remove(card);
        cardTypeDictionary[card.cardData.cardType]?.Remove(card);
        ArrangeCards();
    }
    //Power-ups
    private (CardType, int) GetMostFrequentCardType() {
        CardType magicCardType = CardType.nothing;
        int maxCount = 0;

        foreach (var cardType in cardTypeDictionary) {
            if (cardType.Value.Count > maxCount) {
                maxCount = cardType.Value.Count;
                magicCardType = cardType.Key;
            }
        }
        return (magicCardType, maxCount);
    }

    public bool StackMagicHandler() {
        var (magicCardType, maxCount) = GetMostFrequentCardType();
        int magicCardAmount = 3 - maxCount;

        if (magicCardAmount <= currentSizeStack - cardsInStack.Count) {
            GameEvents.OnMagicPowerClickedInvoke(magicCardType, magicCardAmount);
            return true;
        }
        return false;
    }

    public void ShuffleMagicHandler() {
        var (magicCardType, maxCount) = GetMostFrequentCardType();
        GameEvents.OnShufflePowerClickedInvoke(magicCardType, 3 - maxCount);
    }

    public void AddOneCell() {
        Debug.Log("Added");
        currentSizeStack++;
    } 
}
