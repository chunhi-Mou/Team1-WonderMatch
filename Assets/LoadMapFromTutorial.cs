using UnityEngine;

public class LoadMapFromTutorial : MonoBehaviour {
    public void LoadMap() {
        SceneLoader.instance.LoadScene("Map");
    }
}
