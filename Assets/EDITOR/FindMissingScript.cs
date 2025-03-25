using UnityEngine;
using UnityEditor;

public class FindMissingScript : MonoBehaviour {
    [MenuItem("Tools/Find Missing Scripts")]
    public static void Find() {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        int missingCount = 0;

        foreach (GameObject obj in allObjects) {
            Component[] components = obj.GetComponents<Component>();

            foreach (Component comp in components) {
                if (comp == null) {
                    Debug.LogWarning($"Missing Script on: {obj.name}", obj);
                    missingCount++;
                }
            }
        }

        Debug.Log($"Tìm thấy {missingCount} Missing Scripts trong Scene!");
    }
}
