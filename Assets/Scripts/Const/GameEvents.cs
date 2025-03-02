using System;
using UnityEngine;

public static class GameEvents{
    public static event Action<GameObject> OnTileSelected;
    public static event Action<Transform> OnFoundPosOfTile;

    public static void OnTileSelectedInvoke(GameObject selectedTile) {
        OnTileSelected.Invoke(selectedTile);
    }
    public static void OnFoundPosOfTileInvoke(Transform target) {
        OnFoundPosOfTile.Invoke(target);
    }
}
