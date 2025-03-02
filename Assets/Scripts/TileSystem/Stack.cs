using System.Collections.Generic;
using UnityEngine;

public class Stack : MonoBehaviour
{
    [SerializeField] Transform[] centerPos;
    [SerializeField] List<Tile> tilesInStack;
    
    private Dictionary<CardType, List<Tile>> tileDictionary; 

    public int maxSizeStack = 8;
    public int currentSizeStack = 7;

    private void Start()
    {
        tilesInStack = new List<Tile>();
        tileDictionary = new Dictionary<CardType, List<Tile>>();
    }

    private void OnEnable()
    {
        GameEvents.OnTileSelected += GetTileTargetPos;
        GameEvents.OnTileDoneMoving += CheckMatch;
    }

    private void OnDisable()
    {
        GameEvents.OnTileSelected -= GetTileTargetPos;
        GameEvents.OnTileDoneMoving -= CheckMatch;
    }

    private void GetTileTargetPos(Tile tile)
    {
        int targetIndex = tilesInStack.Count;

        if (tileDictionary.TryGetValue(tile.card.cardType, out List<Tile> matchingTiles) && matchingTiles.Count > 0)
        {
            targetIndex = tilesInStack.IndexOf(matchingTiles[matchingTiles.Count - 1]) + 1;
        }

        AddTileToStack(targetIndex, tile);
        tile.MoveTileTo(centerPos[targetIndex]);
    }

    private void CheckFullStack()
    {
        if (tilesInStack.Count == currentSizeStack) Debug.Log("Stack Is Full!");
    }

    private void CheckMatch()
    {
        int currentMatchPos = tilesInStack.Count;
        for (int i = tilesInStack.Count - 1; i >= 2; i--)
        {
            if (tilesInStack[i].card.cardType == tilesInStack[i - 1].card.cardType &&
                tilesInStack[i].card.cardType == tilesInStack[i - 2].card.cardType)
            {
                currentMatchPos = i - 2;
                Debug.Log("Match 3 Found!");
                RemoveMatchFromStack(currentMatchPos);
            }
        }
        CheckFullStack();
    }

    private void AddTileToStack(int targetIndex, Tile tile)
    {
        tilesInStack.Insert(targetIndex, tile);

        if (!tileDictionary.ContainsKey(tile.card.cardType))
        {
            tileDictionary[tile.card.cardType] = new List<Tile>();
        }
        tileDictionary[tile.card.cardType].Add(tile);
    }

    private void RemoveMatchFromStack(int currentMatchPos)
    {
        CardType removedCardType = tilesInStack[currentMatchPos].card.cardType;

        for (int i = 0; i < 3; i++)
        {
            Tile tile = tilesInStack[currentMatchPos];
            tilesInStack.RemoveAt(currentMatchPos);
            tileDictionary[tile.card.cardType].Remove(tile);
        }
    }
}
