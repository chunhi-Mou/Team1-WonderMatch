using System;
using UnityEngine;

public static class GameEvents{
    public static event Action OnTileSelected;
    public static event Action<Transform> OnFoundPosOfTile;

    public static void OnTileSelectedInvoke() {
        OnTileSelected.Invoke();
    }

    public static void OnFoundPosOfTileInvoke(Transform target) {
        OnFoundPosOfTile.Invoke(target);
    }
}
