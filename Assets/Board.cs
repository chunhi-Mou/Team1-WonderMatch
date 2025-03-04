using UnityEngine;

public class Board : MonoBehaviour {
    private void Start() {
        UpdateBoard();
    }
    public void UpdateBoard() {
        GameObject[] cards = GameObject.FindGameObjectsWithTag("Card");
        foreach (GameObject card in cards) {
            CardOverlapChecker checker = card.GetComponent<CardOverlapChecker>();
            checker.CheckIfUncovered();
            Debug.Log(card.name);
        }

    }
}
