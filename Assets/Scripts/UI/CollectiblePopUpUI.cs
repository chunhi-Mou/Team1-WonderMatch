using DG.Tweening;
using UnityEngine;

public class CollectiblePopUpUI : MonoBehaviour
{
    [SerializeField] GameObject[] collectibles;
    private void Start()
    {
        for (int i = 0; i < collectibles.Length; i++)
        {
            GameObject collectibleImg = collectibles[i].transform.Find("Image").gameObject;

            collectibleImg.transform.DOScale(Vector3.zero, 0f);
            collectibles[i].SetActive(false);
        }
    }
    public void ShowCollectible (int number)
    {
        GameObject collectibleImg = collectibles[number - 1].transform.Find("Image").gameObject;
    
        collectibleImg.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
        collectibles[number - 1].SetActive(true);
    }
    public void HideCollectible (int number)
    {
        GameObject collectibleImg = collectibles[number - 1].transform.Find("Image").gameObject;

        collectibleImg.transform.DOScale(Vector3.zero, 0.3f)
            .SetEase(Ease.InBack)
            .OnComplete(() => 
            {
                collectibles[number - 1].SetActive(false);
            });
    }
}
