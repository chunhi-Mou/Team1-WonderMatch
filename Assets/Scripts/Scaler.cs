using UnityEngine;

public class Scaler : MonoBehaviour {
    public float referenceWidth = 1080f;
    public float referenceHeight = 1920f;
    [Range(0f, 1f)] public float biasFactor = 0.85f;

    void Awake() {
        Rect safeArea = Screen.safeArea;

        float safeWidth = safeArea.width;
        float safeHeight = safeArea.height;

        float screenAspect = safeWidth / safeHeight;
        float referenceAspect = referenceWidth / referenceHeight;

        float widthScale = safeWidth/referenceWidth * biasFactor;
        float heightScale = referenceHeight / safeHeight * 1f;

        float finalScale;

        if (screenAspect > referenceAspect) {
            finalScale = widthScale;
        } else {
            finalScale = heightScale;
        }
        finalScale = Mathf.Min(finalScale, biasFactor);
        transform.localScale = new Vector3(finalScale, finalScale, 0.85f);
    }
}
