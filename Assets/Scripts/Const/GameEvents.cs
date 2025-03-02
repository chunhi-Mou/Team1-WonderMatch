using System;
using UnityEngine;

public static class GameEvents{
    public static event Action<CardData> OnTileSelected;
    public static event Action<Transform> OnFoundPosOfTile;

    public static void OnTileSelectedInvoke(CardData cardData) {
        OnTileSelected.Invoke(cardData);
    }
    public static void OnFoundPosOfTileInvoke(Transform target) {
        OnFoundPosOfTile.Invoke(target);
    }
}
