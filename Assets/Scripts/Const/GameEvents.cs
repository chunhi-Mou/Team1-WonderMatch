using System;
using UnityEngine;
using DG.Tweening;

public static class GameEvents {
    // Events
    public static event Action<CardStateManager> OnCardSelected;
    public static event Action OnCardDoneMoving;
    public static event Action OnMatchCards;
    public static event Action OnLoseGame;
    public static event Action OnWinGame;
    public static event Action OnMatchTilesDone;
    public static event Action<CardInfo> OnUndoPressed;
    public static event Action<CardType, int> OnMagicPowerClicked;
    public static event Action OnTurnChange;
    public static event Action<PowerType> OnSpendCoinsNeeded;
    public static event Action<CardType, int> OnShufflePowerClicked;

    public static event Action<Transform> OnCardAddedToStack;
   
    public static void CardAddedToStack(Transform target) {
        OnCardAddedToStack?.Invoke(target);
    }
    // Invoke Methods
    public static void OnMatchTilesDoneInvoke() => OnMatchTilesDone?.Invoke();
    public static void OnCardSelectedInvoke(CardStateManager card) => OnCardSelected?.Invoke(card);
    public static void OnCardDoneMovingInvoke() => OnCardDoneMoving?.Invoke();
    public static void OnMatchCardsInvoke() => OnMatchCards?.Invoke();
    public static void OnLoseGameInvoke() => OnLoseGame?.Invoke();
    public static void OnWinGameInvoke() => OnWinGame?.Invoke();
    public static void OnUndoPressedInvoke(CardInfo card) => OnUndoPressed?.Invoke(card);
    public static void OnMagicPowerClickedInvoke(CardType cardType, int neededCard) => OnMagicPowerClicked?.Invoke(cardType, neededCard);
    public static void OnShufflePowerClickedInvoke(CardType cardType, int neededCard) => OnShufflePowerClicked?.Invoke(cardType, neededCard);
    public static void OnTurnChangeInvoke() => OnTurnChange?.Invoke();
    public static void OnSpendCoinsNeededInvoke(PowerType type) => OnSpendCoinsNeeded?.Invoke(type);
}