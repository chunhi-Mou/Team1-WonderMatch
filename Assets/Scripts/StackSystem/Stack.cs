using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Stack : MonoBehaviour
{
    [SerializeField] Transform[] centerPos; //Thu: Mang gom vi tri cac o trong khay
    [SerializeField] List<Card> cardsInStack; //Thu: Danh sach cac card trong khay
    
    private Dictionary<CardType, List<Card>> cardTypeDictionary; //Thu: Dictionary cua cac loai card khac nhau trong khay

    public int maxSizeStack = 8;
    public int currentSizeStack = 7;

    private void Start()
    {
        cardsInStack = new List<Card>();
        cardTypeDictionary = new Dictionary<CardType, List<Card>>();
    }

    private void OnEnable()
    {
        GameEvents.OnCardSelected += GetCardTargetPos;
        GameEvents.OnCardDoneMoving += CheckMatch;
    }

    private void OnDisable()
    {
        GameEvents.OnCardSelected -= GetCardTargetPos;
        GameEvents.OnCardDoneMoving -= CheckMatch;
    }

    private void GetCardTargetPos(Card card)
    {
        int targetIndex = cardsInStack.Count;

        if (cardTypeDictionary.TryGetValue(card.cardData.cardType, out List<Card> sameTypeCards) && sameTypeCards.Count > 0)
        {
            targetIndex = cardsInStack.IndexOf(sameTypeCards[sameTypeCards.Count - 1]) + 1;
        }

        AddCardToStack(targetIndex, card);
        card.MoveCardTo(centerPos[targetIndex]);
    }

    private void CheckFullStack()
    {
        if (cardsInStack.Count == currentSizeStack) Debug.Log("Stack Is Full!");
    }

    private void CheckMatch()
    {
        int currentMatchPos = cardsInStack.Count;
        for (int i = cardsInStack.Count - 1; i >= 2; i--)
        {
            if (cardsInStack[i].cardData.cardType == cardsInStack[i - 1].cardData.cardType &&
                cardsInStack[i].cardData.cardType == cardsInStack[i - 2].cardData.cardType)
            {
                currentMatchPos = i - 2;
                Debug.Log("Match 3 Found!");
                RemoveMatchFromStack(currentMatchPos);
            }
        }
        CheckFullStack();
    }

    private void AddCardToStack(int targetIndex, Card card)
    {
        cardsInStack.Insert(targetIndex, card);

        //Thu: Neu chua co card nao trong dict cung loai thi tao list moi chua no 
        if (!cardTypeDictionary.ContainsKey(card.cardData.cardType)){
            cardTypeDictionary[card.cardData.cardType] = new List<Card>();
        }

        cardTypeDictionary[card.cardData.cardType].Add(card);
    }

    private void RemoveMatchFromStack(int currentMatchPos)
    {
        List<Card> matchedCards = cardsInStack.GetRange(currentMatchPos, 3);

        foreach (Card card in matchedCards)
        {
            // Thêm hiệu ứng biến mất
            card.transform.DOScale(Vector3.zero, 0.5f).OnComplete(() =>
            {
                cardsInStack.Remove(card);
                cardTypeDictionary[card.cardData.cardType].Remove(card);
                card.DisableMatchedCard(card);
            });
        }

        matchedCards.Clear();
    }
    private void ArrangeCards()
    {
        
    }
}
