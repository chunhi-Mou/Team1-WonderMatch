using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Stack : MonoBehaviour
{
    [SerializeField] Transform[] centerPos;
    [SerializeField] List<Card> cardsInStack = new List<Card>();

    private Dictionary<CardType, List<Card>> cardTypeDictionary = new Dictionary<CardType, List<Card>>();
    private Queue<Card> pendingCards = new Queue<Card>(); 
    private bool isArranging = false; 

    public int maxSizeStack = 8;
    public int currentSizeStack = 7;

    private void OnEnable()
    {
        GameEvents.OnUndoPressed += RemoveUndoCard;
        GameEvents.OnMatchCards += ArrangeCards;
        GameEvents.OnCardSelected += GetCardTargetPos;
        GameEvents.OnCardDoneMoving += CheckMatch;
    }

    private void OnDisable()
    {
        GameEvents.OnUndoPressed -= RemoveUndoCard;
        GameEvents.OnMatchCards -= ArrangeCards;
        GameEvents.OnCardSelected -= GetCardTargetPos;
        GameEvents.OnCardDoneMoving -= CheckMatch;
    }
    public void AddOneCell()
    {
        currentSizeStack += 1;
    }
    private void GetCardTargetPos(Card card)
    {
        if (!card) return;

        if (isArranging)
        {
            pendingCards.Enqueue(card);
            return;
        }

        int targetIndex = cardsInStack.Count;
        if (cardTypeDictionary.TryGetValue(card.cardData.cardType, out var sameTypeCards) && sameTypeCards.Count > 0)
        {
            targetIndex = cardsInStack.IndexOf(sameTypeCards[^1]) + 1;
        }

        AddCardToStack(targetIndex, card);
    }

    private void CheckFullStack()
    {
        if (cardsInStack.Count >= currentSizeStack) {
            Debug.Log("Stack Is Full!");
            GameEvents.OnLoseGameInvoke();
        }
    }

    private void CheckMatch()
    {
        if (cardsInStack.Count < 3) return;

        for (int i = 0; i <= cardsInStack.Count - 3; i++)
        {
            var cardType = cardsInStack[i].cardData.cardType;
            if (cardType == cardsInStack[i + 1].cardData.cardType && cardType == cardsInStack[i + 2].cardData.cardType)
            {
                RemoveMatchFromStack(i);
                return;
            }
        }
        CheckFullStack();
    }

    private void AddCardToStack(int targetIndex, Card card)
    {
        cardsInStack.Insert(targetIndex, card);
        cardTypeDictionary.TryAdd(card.cardData.cardType, new List<Card>());
        cardTypeDictionary[card.cardData.cardType].Add(card);

        for (int i = targetIndex; i < cardsInStack.Count; i++)
        {
            cardsInStack[i].MoveCardTo(centerPos[i].position,0.1f);
        }
        card.MoveCardTo(centerPos[targetIndex].position,0.8f);
    }

    private void RemoveMatchFromStack(int currentMatchPos)
    {
        GameEvents.OnMatchCardsInvoke();
        List<Card> matchedCards = cardsInStack.GetRange(currentMatchPos, 3);
        
        int completedAnimations = 0;
        int totalMatches = matchedCards.Count;
        isArranging = true; 

        foreach (var card in matchedCards)
        {
            cardsInStack.Remove(card);
            cardTypeDictionary[card.cardData.cardType]?.Remove(card);
            card.transform.DOScale(Vector3.zero, 0.5f).OnComplete(() =>
            {
                card.DisableMatchedCard(card);
                completedAnimations++;

                if (completedAnimations == totalMatches)
                {
                    isArranging = false; 
                    ArrangeCards();
                    ProcessPendingCards();
                    GameEvents.OnMatchTilesDoneInvoke();
                }
            });
        }
    }

    private void ArrangeCards()
    {
        for (int i = 0; i < cardsInStack.Count; i++)
        {
            cardsInStack[i].MoveCardTo(centerPos[i].position,0.3f);
        }
    }

    private void ProcessPendingCards()
    {
        while (pendingCards.Count > 0)
        {
            GetCardTargetPos(pendingCards.Dequeue());
        }
    }
    public bool StackMagicHandler()
    {
        CardType magicCardType = CardType.nothing;
        int maxCount = 0;

        foreach (var cardType in cardTypeDictionary)
        {
            if (cardType.Value.Count > maxCount)
            {
                maxCount = cardType.Value.Count;
                magicCardType = cardType.Key;
            }
        }

        int magicCardAmount = 3 - maxCount;
        int availableStackCount = currentSizeStack - cardsInStack.Count;

        if (magicCardAmount <= availableStackCount) {
            GameEvents.OnMagicPowerClickedInvoke(magicCardType, magicCardAmount);
            return true;
        } else {
            return false;
        }
    }
    
    public void ShuffleMagicHandler()
    {
        CardType magicCardType = CardType.nothing;
        int maxCount = 0;

        foreach (var cardType in cardTypeDictionary)
        {
            if (cardType.Value.Count > maxCount)
            {
                maxCount = cardType.Value.Count;
                magicCardType = cardType.Key;
            }
        }

        int magicCardAmount = 3 - maxCount;

        GameEvents.OnShufflePowerClickedInvoke(magicCardType, magicCardAmount);
    }

    private void RemoveUndoCard(Card card)
    {
        cardsInStack.Remove(card);
        cardTypeDictionary[card.cardData.cardType].Remove(card);
        ArrangeCards();
    }
}