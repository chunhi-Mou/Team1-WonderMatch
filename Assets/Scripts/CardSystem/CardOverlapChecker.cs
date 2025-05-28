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

    public void UpdateBelowTiles(float _deep = 5f) {
        cardsBelow.Clear();

        float shrinkFactor = 0.01f;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if (sr == null || sr.sprite == null) {
            return;
        }

        Vector3 spriteWorldSize = sr.bounds.size;

        Vector3 boxFullSize = new Vector3(
            spriteWorldSize.x - shrinkFactor,
            spriteWorldSize.y - shrinkFactor,
            _deep                             
        );

        Vector3 boxCenterPosition = transform.position + new Vector3(0, 0, _deep / 2f);

        Collider[] colliders = Physics.OverlapBox(boxCenterPosition, boxFullSize / 2f, Quaternion.identity);

        foreach (var col in colliders) {
            if (col.gameObject != gameObject) {
                Card otherCard = col.GetComponent<Card>();
                if (otherCard != null) {
                    cardsBelow.Add(otherCard);
                }
            }
        }
    }

    public void UpdateAboveTiles(float _deep = 5f) {
        cardsAbove.Clear();

        float shrinkFactor = 0.01f;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if (sr == null || sr.sprite == null) {
            return;
        }

        Vector3 spriteWorldSize = sr.bounds.size;

        Vector3 boxFullSize = new Vector3(
            spriteWorldSize.x - shrinkFactor,
            spriteWorldSize.y - shrinkFactor,
            _deep
        );
        Vector3 boxCenterPosition = transform.position + new Vector3(0, 0, -_deep / 2f);

        Collider[] colliders = Physics.OverlapBox(boxCenterPosition, boxFullSize / 2f, Quaternion.identity);

        foreach (var col in colliders) {
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
            t.GetComponent<CardOverlapChecker>()?.CheckIfUncovered();
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

    private void OnDrawGizmosSelected() {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null || sr.sprite == null) return;

        float drawDeep = 5f;
        float shrinkFactor = 0.01f;
        Vector3 spriteWorldSize = sr.bounds.size;

        Vector3 belowBoxFullSize = new Vector3(spriteWorldSize.x - shrinkFactor, spriteWorldSize.y - shrinkFactor, drawDeep);
        Vector3 belowBoxCenterPosition = transform.position + new Vector3(0, 0, drawDeep / 2f);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(belowBoxCenterPosition, belowBoxFullSize);

        Vector3 aboveBoxFullSize = new Vector3(spriteWorldSize.x - shrinkFactor, spriteWorldSize.y - shrinkFactor, drawDeep);
        Vector3 aboveBoxCenterPosition = transform.position + new Vector3(0, 0, -drawDeep / 2f);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(aboveBoxCenterPosition, aboveBoxFullSize);
    }
}