using System.Collections.Generic;
using UnityEngine;

public class CardOverlapChecker : MonoBehaviour {
    public List<CardStateManager> cardsBelow = new();
    public List<CardStateManager> cardsAbove = new();
    const float SHRINK_FACTOR = 0.01f;

    public List<CardStateManager> UpdateTiles(List<CardStateManager> cardsList, float _deep = 1f, bool isBelow = true) {
        cardsList.Clear();

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Vector3 spriteSize = sr.bounds.size;
        Vector3 scaledSize = new Vector3(spriteSize.x * transform.localScale.x, spriteSize.y * transform.localScale.y, _deep);
        Vector3 boxSize = scaledSize - new Vector3(SHRINK_FACTOR, SHRINK_FACTOR, 0);
        Vector3 boxPosition = transform.position + new Vector3(0, 0, isBelow ? _deep : -_deep);

        Collider[] cards = Physics.OverlapBox(boxPosition, boxSize / 2, Quaternion.identity);

        foreach (var col in cards) {
            CardStateManager otherCard = col.GetComponent<CardStateManager>();
            if (otherCard != null) cardsList.Add(otherCard);
        }
        return cardsList;
    }

    public List<CardStateManager> UpdateBelowCards() => UpdateTiles(cardsBelow,1f, true);
    public List<CardStateManager> UpdateAboveCards() => UpdateTiles(cardsAbove,1f, false);

    public bool IsNoAboveCards() {
        UpdateAboveCards();
        return cardsAbove.Count == 0;
    }
    public bool IsNoBelowCards() {
        UpdateBelowCards();
        return cardsBelow.Count == 0;
    }
}
