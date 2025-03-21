using UnityEngine;

public class LevelLoader : MonoBehaviour {
    public Transform levelParent;

    private void Start() {
        CoinsManager.Instance.ToggleCoinsUI(false);
        int selectedLevel = LevelManager.CurrLevel;
        string prefabName = "LevelPrefabs/Lv" + selectedLevel;
        GameObject levelPrefab = Resources.Load<GameObject>(prefabName);
        if (levelPrefab != null) {
            Instantiate(levelPrefab, levelParent);
        }
    }
}
