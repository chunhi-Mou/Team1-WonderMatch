using UnityEngine;

public class LevelLoader : MonoBehaviour {
    public Transform levelParent;

    private void Start() {
        int selectedLevel = LevelManager.CurrLevel;
        string prefabName = "LevelPrefabs/Level" + selectedLevel;
        GameObject levelPrefab = Resources.Load<GameObject>(prefabName);
        if (levelPrefab != null) {
            Instantiate(levelPrefab, levelParent);
        }
    }
}
