using System.Collections.Generic;
using UnityEngine;

public class Stack : MonoBehaviour
{
    [SerializeField] Transform[] centerPos;
    [SerializeField] List<Tile> tilesInStack;
    public int maxSizeStack = 8;
    public int currentSizeStack = 7;
    private void Start()
    {
        tilesInStack = new List<Tile>();
    }
    private void OnEnable()
    {
        GameEvents.OnTileSelected+=GetTileTargetPos;
        GameEvents.OnTileDoneMoving+=CheckMatch;

    }
    private void OnDisable()
    {
        GameEvents.OnTileSelected-=GetTileTargetPos;
        GameEvents.OnTileDoneMoving-=CheckMatch;
    }
    private void GetTileTargetPos(Tile tile)
    {
        int targetIndex=tilesInStack.Count; 
        for (int i=tilesInStack.Count - 1; i >= 0; i --){
            if (tile.card.cardType == tilesInStack[i].card.cardType) {
                targetIndex = i+1;
                break;
            }
        }
        GameEvents.OnFoundPosOfTileInvoke(centerPos[targetIndex]);
        AddTileToStack(targetIndex, tile);
    }
    private void CheckFullStack()
    {
        if (tilesInStack.Count == currentSizeStack) Debug.Log("Stack Is Full!");
    }
    private void CheckMatch()
    {
        int currentMatchPos = tilesInStack.Count;
        for (int i=tilesInStack.Count - 1; i >= 2; i --) {
            if (tilesInStack[i].card.cardType == tilesInStack[i-1].card.cardType && tilesInStack[i].card.cardType == tilesInStack[i-2].card.cardType){
                currentMatchPos=i-2;
                Debug.Log("Match 3 Found!");
                RemoveMatchFromStack(currentMatchPos);
            }
        }
        CheckFullStack();
    }
    /// <param name="targetIndex">vi tri ben phai cua tile giong tile minh can insert</param>
    private void AddTileToStack(int targetIndex, Tile tile) 
    {
        tilesInStack.Insert(targetIndex, tile);
    }
    private void RemoveMatchFromStack(int currentMatchPos)
    {
        tilesInStack.RemoveRange(currentMatchPos,3);
    }
}
