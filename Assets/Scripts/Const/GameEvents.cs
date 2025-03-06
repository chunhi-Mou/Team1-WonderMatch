using System;
using UnityEngine;
using DG.Tweening;

public static class GameEvents {
    public static event Action<Card> OnCardSelected;
    public static event Action<Transform, float, Ease> OnFoundPosOfCard;
    public static event Action OnCardDoneMoving;
    public static event Action OnLoseGame;

    public static void OnCardSelectedInvoke(Card card) {
        OnCardSelected?.Invoke(card);
    }
    public static void OnFoundPosOfCardInvoke(Transform target, float _duration, Ease _ease) {
        OnFoundPosOfCard?.Invoke(target, _duration, _ease);
    }
    public static void OnCardDoneMovingInvoke() {
        OnCardDoneMoving?.Invoke();
    }
    public static void OnLoseGameInvoke() {
        OnLoseGame?.Invoke();
    }
}
