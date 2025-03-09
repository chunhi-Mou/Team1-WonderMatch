using UnityEngine;

public class PowerUps : MonoBehaviour {
    public Board board;
    public Card prevCard;
    private void Awake() {
        if (board == null) {
            board = FindObjectOfType<Board>(); 
        }
    }

    private void OnEnable() {
        GameEvents.OnCardSelected += SetPrevCard;
    }
    private void OnDisable() {
        GameEvents.OnCardSelected -= SetPrevCard;
    }
    public void OnShufflePress() {
        board.ShuffleBoard();
    }
    public void SetPrevCard(Card card) {
        prevCard = card;
    }
    public void OnUndoPress() {
        if (prevCard != null) {
            prevCard.UndoMove();
        } else {
            //Gọi Stack trả về cùng phải
        }
    }
}
