using UnityEngine;
using DG.Tweening;

public class PaperAnim : MonoBehaviour {
    public Transform PaperUp;
    public Transform PaperDown;
    public float moveDuration = 0.25f;

    private void OnEnable() {
        gameObject.transform.position = PaperUp.position;
        transform.DOMove(PaperDown.position, moveDuration)
                 .SetEase(Ease.OutQuad).SetUpdate(true);
    }
}
