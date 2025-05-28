using UnityEngine;
using UnityEngine.UI;

public class CollectionLoader : MonoBehaviour {
    [SerializeField] Button collectableCard;
    [SerializeField] CardType cardType;
    private void Start() {
        int state = PlayerPrefs.GetInt("SCard" + (int)cardType, 0);
        if(state==1) {
            collectableCard.interactable = true;
        } else {
            collectableCard.interactable = false;
        }
    }
}
