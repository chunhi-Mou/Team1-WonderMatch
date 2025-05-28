using UnityEngine;
using UnityEngine.UI;

public class ButtonLvObj : MonoBehaviour {
    public Button levelButton;
    public Image[] stars;

    private void Awake() {
        if (levelButton == null) levelButton = GetComponent<Button>();

        if ((stars == null || stars.Length == 0)) {
            Transform starsParent = transform.Find("stars");
            if (starsParent != null) {
                int count = starsParent.childCount;
                stars = new Image[count];
                for (int i = 0; i < count; i++) {
                    stars[i] = starsParent.GetChild(i).GetComponent<Image>();
                }
            }
        }
    }

    public void UpdateStars(int starCount, Sprite fullStar, Sprite emptyStar) {
        for (int i = 0; i < stars.Length; i++) {
            stars[i].sprite = (i < starCount) ? fullStar : emptyStar;
        }
    }
}
