using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Card))]
public class CardOverlapChecker : MonoBehaviour {
    private Card card;
    private List<Card> cardsBelow = new List<Card>();
    private List<Card> cardsAbove = new List<Card>(); 

    private BoxCollider boxCollider;
    private void Awake() {
        card = GetComponent<Card>();
        boxCollider = GetComponent<BoxCollider>();
    }
    public void UpdateBelowTiles(float _deep = 1f) {
        cardsBelow.Clear();

        float shrinkFactor = 0.01f;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Vector3 spriteSize = sr.bounds.size; 
        Vector3 scaledSize = new Vector3(spriteSize.x * transform.localScale.x, spriteSize.y * transform.localScale.y, _deep);
        Vector3 belowBoxSize = scaledSize - new Vector3(shrinkFactor, shrinkFactor, 0);
        Vector3 belowBoxPosition = transform.position + new Vector3(0, 0, _deep);

        Collider[] cards = Physics.OverlapBox(belowBoxPosition, belowBoxSize / 2, Quaternion.identity);

        foreach (var col in cards) {
            if (col.gameObject != gameObject) {
                Card otherCard = col.GetComponent<Card>();
                if (otherCard != null) {
                    cardsBelow.Add(otherCard);
                }
            }
        }
    }

    public void UpdateAboveTiles(float _deep = 1f) {
        cardsAbove.Clear();

        float shrinkFactor = 0.01f;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Vector3 spriteSize = sr.bounds.size; 
        Vector3 scaledSize = new Vector3(spriteSize.x * transform.localScale.x, spriteSize.y * transform.localScale.y, _deep);
        Vector3 aboveBoxSize = scaledSize - new Vector3(shrinkFactor, shrinkFactor, 0);
        Vector3 aboveBoxPosition = transform.position + new Vector3(0, 0, -_deep);

        Collider[] cards = Physics.OverlapBox(aboveBoxPosition, aboveBoxSize / 2, Quaternion.identity);

        foreach (var col in cards) {
            if (col.gameObject != gameObject) {
                Card otherCard = col.GetComponent<Card>();
                if (otherCard != null) {
                    cardsAbove.Add(otherCard);
                }
            }
        }
    }


    public void NotifyTilesBelow() {
        foreach (var t in cardsBelow) {
            t.GetComponent<CardOverlapChecker>().CheckIfUncovered();
        }
    }
    public void CheckIfUncovered() {
        UpdateAboveTiles();
        if (cardsAbove.Count == 0) {
            card.SetSelectableData(true);
        } else {
            card.SetSelectableData(false);
        }
    }
}
