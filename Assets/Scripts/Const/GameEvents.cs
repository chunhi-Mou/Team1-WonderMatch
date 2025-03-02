using System;
using UnityEngine;

public static class GameEvents{
    public static event Action<Tile> OnTileSelected;
    public static event Action<Transform> OnFoundPosOfTile;
    public static event Action OnTileDoneMoving;
    public static event Action OnLoseGame;

    public static void OnTileSelectedInvoke(Tile tile) {
        OnTileSelected.Invoke(tile);
    }
    public static void OnFoundPosOfTileInvoke(Transform target) {
        OnFoundPosOfTile.Invoke(target);
    }
    public static void OnTileDoneMovingInvoke(){
        OnTileDoneMoving.Invoke();
    }
    public static void OnLoseGameInvoke(){
        OnLoseGame.Invoke();
    }
}
