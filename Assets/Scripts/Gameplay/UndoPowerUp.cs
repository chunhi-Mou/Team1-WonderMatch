using UnityEngine;

public class UndoPowerUp : IPowerUp {
    public void OnEnable() {
    }
    public void OnDisable() {
    }
    private int count = 3;
    public UndoPowerUp() {
        count = PlayerPrefs.GetInt(SavedData.UndoPowerCount, 3);
    }
    public void Use() {
        if (count > 0) {
            CardStateManager lastCard = CardHistory.Instance.UndoLastMove();
            if (lastCard != null) {
                AudioManager.instance.Play(SoundEffect.Undo);
                //lastCard.UndoMove();
                count--;
                SaveData();
            }
        } else {
            GameEvents.OnSpendCoinsNeededInvoke(PowerType.Undo);
        }
    }

    public void ResetCount(int maxCount) {
        count = maxCount;
    }

    public int GetCount() {
        return count;
    }
    public void SaveData() {
        PlayerPrefs.SetInt(SavedData.UndoPowerCount, count);
        PlayerPrefs.Save();
    }
}
//public void UndoMove() {
//    if (!GameModeManager.instance.isProcessingCard && state != CardState.inStack) return;
//    GameEvents.OnUndoPressedInvoke(this);
//    GetComponent<Collider>().enabled = true;
//    CardAnimation.PlayCardShakeThenMove(this.transform, prevPosition, 0.3f, 0.5f, () => {
//        HandleCardMoveComplete();
//        GameModeManager.instance.isUsingPowers = false;
//    });
//    state = CardState.inBoard;
//    this.isSelectable = true;
//}
