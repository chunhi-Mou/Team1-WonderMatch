using UnityEngine;

[CreateAssetMenu(fileName = "CardDatabase", menuName = "Game/Card Database")]
public class CardDatabase : ScriptableObject {
    public CardData[] cards;
}

[System.Serializable]
public class CardData {
    public CardType cardType;
    public Sprite sprite;
}
