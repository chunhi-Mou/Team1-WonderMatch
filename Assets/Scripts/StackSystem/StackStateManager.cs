using System.Collections.Generic;
using UnityEngine;

public class StackStateManager : MonoBehaviour
{
    public Transform[] centerPos;
    public List<CardStateManager> cardsInStack = new List<CardStateManager>();
    private Dictionary<CardType, List<CardStateManager>> cardTypeDictionary = new Dictionary<CardType, List<CardStateManager>>();
    public StackBaseState currState;
    public StackIdleState idleState = new StackIdleState();
    public StackAddingState addingState = new StackAddingState();
    public StackCheckMatchState checkMatchState = new StackCheckMatchState();
    public int currentSizeStack = 7;

    private void OnEnable()
    {
        GameEvents.OnAddingCard += AddingCardHandler;
        GameEvents.OnCardDoneMoving += CheckMatchHandler;
    }
    private void OnDisable()
    {
        GameEvents.OnAddingCard -= AddingCardHandler;
    }
    private void Start()
    {
        SwitchState(idleState);

    }
    private void AddingCardHandler(CardStateManager card)
    {
        SwitchState(addingState);
        GetCardTargetPos(card);
    }
    private void CheckMatchHandler()
    {
        SwitchState(checkMatchState);
        CheckMatch();
    }
    public void SwitchState(StackBaseState state) {
        currState = state;
        currState.EnterState(this);
    }
    private void GetCardTargetPos(CardStateManager card) {
        // if (!card) return;

        // if (isArranging || isAddingCard) {
        //     pendingCards.Enqueue(card);
        //     return;
        // }

        int targetIndex = cardsInStack.Count;
        if (cardTypeDictionary.TryGetValue(card.cardInfo.cardData.cardType, out var sameTypeCards) && sameTypeCards.Count > 0) {
            targetIndex = cardsInStack.IndexOf(sameTypeCards[^1]) + 1;
        }

        AddCardToStack(targetIndex, card);
    }
    private void AddCardToStack(int targetIndex, CardStateManager card) {
        cardsInStack.Insert(targetIndex, card);
        cardTypeDictionary.TryAdd(card.cardInfo.cardData.cardType, new List<CardStateManager>());
        cardTypeDictionary[card.cardInfo.cardData.cardType].Add(card);

        StackAnimation.AnimateAddCard(cardsInStack, centerPos, targetIndex, () => {
            card.target = centerPos[targetIndex].position;
            card.currState.UpdateState(card);
        });
    }
    private void CheckMatch() {
        //if (isAddingCard || isArranging || cardsInStack.Count < 3) return;

        for (int i = 0; i <= cardsInStack.Count - 3; i++) {
            var cardType = cardsInStack[i].cardInfo.cardData.cardType;
            if (cardType == cardsInStack[i + 1].cardInfo.cardData.cardType && cardType == cardsInStack[i + 2].cardInfo.cardData.cardType) {
                RemoveMatchFromStack(i);
                return;
            }
        }
        CheckFullStack();
    }
    private void CheckFullStack() {
        if (cardsInStack.Count >= currentSizeStack) {
            Debug.Log("Stack Is Full!");
            GameEvents.OnLoseGameInvoke();
        }
    }
    private void RemoveMatchFromStack(int currentMatchPos) {
        GameEvents.OnMatchCardsInvoke();
        List<CardStateManager> matchedCards = cardsInStack.GetRange(currentMatchPos, 3);
        //isArranging = true;

        //StackAnimation.AnimateRemoveMatch(matchedCards, () => {
            foreach (var card in matchedCards) {
                cardsInStack.Remove(card);
                cardTypeDictionary[card.cardInfo.cardData.cardType]?.Remove(card);
                card.SwitchState(card.matchedState);
            }

            //isArranging = false;
            ArrangeCards();
            //ProcessPendingCards();
            GameEvents.OnMatchTilesDoneInvoke();
        //});
    }
    private void ArrangeCards() {
        StackAnimation.AnimateArrangeCards(cardsInStack, centerPos);
    }
}