using UnityEngine;
using UnityEngine.UI;

public class CollectionLoader : MonoBehaviour {
    [SerializeField] Image collectableCard;
    [SerializeField] Sprite collected;
    [SerializeField] Sprite uncollected;
    [SerializeField] CardType cardType;

    private void Start() {
        int state = PlayerPrefs.GetInt("SCard" + (int)cardType, 0);
        Debug.Log(cardType.ToString());
        if(state==1) {
            collectableCard.sprite = collected;
        } else {
            collectableCard.sprite = uncollected;
        }
    }
}
