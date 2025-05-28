using DG.Tweening;
using UnityEngine;

public class DuoModeManager : MonoBehaviour, IGameMode {
    #region Singleton - Dont destroy
    public static DuoModeManager instance { get; private set; }

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }
    }
    #endregion
    public void TurnOnObjsOfMode() {

    }
    public void ClearOldData() {
        DOTween.KillAll();
    }
}
