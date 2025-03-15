using UnityEngine;

public class Hearts : MonoBehaviour {
    [SerializeField] int currHeart = 0;
    public int maxHeart = 4;
    public float healTime = 30f;
    public void AddHeart(int _hearts) {
        currHeart += _hearts;
        SaveHearts(currHeart);
    }
    private void SaveHearts(int _heart) {
        PlayerPrefs.SetInt(SavedData.Hearts, _heart);
        PlayerPrefs.Save();
    }
}
