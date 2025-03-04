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
    public void UpdateBelowTiles() {
        cardsBelow.Clear();

        Vector3 belowBoxSize = new Vector3(boxCollider.size.x, boxCollider.size.y, 1f);
        Vector3 belowBoxPosition = transform.position + new Vector3(0, 0, -1f); 

        Collider[] results = Physics.OverlapBox(belowBoxPosition, belowBoxSize / 2, Quaternion.identity);

        foreach (var card in results) {
            if (card.gameObject != gameObject) {
                Card otherCard = card.GetComponent<Card>();

                if (otherCard != null) {
                    cardsBelow.Add(otherCard);
                    Debug.Log(otherCard);
                }
            }
        }
    }
    public void UpdateAboveTiles() {
        cardsAbove.Clear();

        Vector3 aboveBoxSize = new Vector3(boxCollider.size.x, boxCollider.size.y, 1f);
        Vector3 aboveBoxPosition = transform.position + new Vector3(0, 0, 1f);

        Collider[] results = Physics.OverlapBox(aboveBoxPosition, aboveBoxSize / 2, Quaternion.identity);

        foreach (var card in results) {
            if (card.gameObject != gameObject) {
                Card otherCard = card.GetComponent<Card>();

                if (otherCard != null) {
                    cardsAbove.Add(otherCard);
                    Debug.Log(otherCard);
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
        }
    }
}
