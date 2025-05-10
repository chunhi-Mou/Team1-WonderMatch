using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BookmarkUI : MonoBehaviour
{
    [SerializeField] Sprite bookmarkDefault;
    [SerializeField] Image bookmarkAnimation;
    [SerializeField] GameObject collection;
    [SerializeField] Button mapButton;
    [SerializeField] Button collectionButton;
    [SerializeField] GameObject nextAndPrev;
    private Animator bookmarkAnimator;
    private void Start()
    {
        bookmarkAnimation.gameObject.SetActive(false);
        bookmarkAnimator = bookmarkAnimation.gameObject.GetComponent<Animator>();
        collection.SetActive(false);
        mapButton.interactable = false;

        mapButton.onClick.RemoveAllListeners();
        collectionButton.onClick.RemoveAllListeners();

        mapButton.onClick.AddListener(ShowMap);
        collectionButton.onClick.AddListener(ShowCollection);
    }
    private void ShowMap()
    {
        mapButton.interactable = false;
        collectionButton.interactable = true;

        bookmarkAnimation.DOFade(0f, 0f);
        bookmarkAnimation.sprite = bookmarkDefault;
        bookmarkAnimation.gameObject.SetActive(true);

        Sequence seq = DOTween.Sequence();
        seq.Append(bookmarkAnimation.DOFade(1f, 0.3f))
        .AppendCallback(() => {
            AudioManager.instance.Play(SoundEffect.bookBackToMap);
            bookmarkAnimator.Play("Bookmark");
            collection.SetActive(false);
            nextAndPrev.SetActive(true);
        }) 
        .AppendInterval(0.6f) 
        .AppendCallback(() => {
            bookmarkAnimation.gameObject.SetActive(false);
        });
    }
    private void ShowCollection()
    {
        GameEvents.OnShowCollectionInvoke();

        collectionButton.interactable = false;
        mapButton.interactable = true;

        bookmarkAnimation.DOFade(0f, 0f);
        bookmarkAnimation.sprite = bookmarkDefault;
        bookmarkAnimation.gameObject.SetActive(true);

        Sequence seq = DOTween.Sequence();
        seq.Append(bookmarkAnimation.DOFade(1f, 0.3f))
        .AppendCallback(() => {
            AudioManager.instance.Play(SoundEffect.bookCollectPage);
            bookmarkAnimator.Play("Bookmark");
            collection.SetActive(true);
            nextAndPrev.SetActive(false);
        }) 
        .AppendInterval(0.6f) 
        .AppendCallback(() => {
            bookmarkAnimation.gameObject.SetActive(false);
        });
    }
}
