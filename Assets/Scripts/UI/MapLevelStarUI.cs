using UnityEngine;

public class MapLevelStarUI : MonoBehaviour
{
    [SerializeField] GameObject[] stars;
    public void ShowStarOnMap(int numberOfStars)
    {
        for (int i = 0; i < numberOfStars; i++)
        {
            stars[i].SetActive(true);
        }
    }
}
