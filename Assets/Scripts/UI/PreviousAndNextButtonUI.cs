using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PreviousAndNextButtonUI : MonoBehaviour
{
    [SerializeField] Button nextButton;
    [SerializeField] Button prevButton;
    [SerializeField] GameObject[] Pages;
    [SerializeField] Image nextAnimation;
    [SerializeField] Image prevAnimation;
    private int currPage;
    private int prevPage = 0;
    private int maxPage;
    private Animator nextAnimator;
    private Animator prevAnimator;
    private void Start()
    {
        nextButton.onClick.RemoveAllListeners();
        prevButton.onClick.RemoveAllListeners();

        nextButton.onClick.AddListener(NextPage);
        prevButton.onClick.AddListener(PrevPage);

        currPage = PlayerPrefs.GetInt(SavedData.CurrPage, 1);
        maxPage = Pages.Length;

        nextAnimation.gameObject.SetActive(false);
        prevAnimation.gameObject.SetActive(false);

        nextAnimator = nextAnimation.GetComponent<Animator>();
        prevAnimator = prevAnimation.GetComponent<Animator>();

        for (int i = 0; i < maxPage; i++) Pages[i].SetActive(false);

        LoadPage();
    }
    private void NextPage()
    {
        if (currPage < maxPage) {
            prevPage = currPage;
            currPage++;
            SavePage();
        }

        nextAnimation.DOFade(0f, 0f);
        nextAnimation.gameObject.SetActive(true);

        Sequence seq = DOTween.Sequence();
        seq.Append(nextAnimation.DOFade(1f, 0.3f))
        .AppendCallback(() => {
            nextAnimator.Play("animate");
            LoadPage();
        }) 
        .AppendInterval(0.6f) 
        .Append(nextAnimation.DOFade(0f, 0.25f)) 
        .AppendCallback(() => {
            nextAnimation.gameObject.SetActive(false);
        });
    }
    private void PrevPage()
    {
        if (currPage > 1) {
            prevPage = currPage;
            currPage--;
            SavePage();
        }

        prevAnimation.DOFade(0f, 0f);
        prevAnimation.gameObject.SetActive(true);

        Sequence seq = DOTween.Sequence();
        seq.Append(prevAnimation.DOFade(1f, 0.3f))
        .AppendCallback(() => {
            prevAnimator.Play("animate");
            LoadPage();
        }) 
        .AppendInterval(0.6f) 
        .Append(prevAnimation.DOFade(0f, 0.25f)) 
        .AppendCallback(() => {
            prevAnimation.gameObject.SetActive(false);
        });
    }
    private void LoadPage()
    {
        if (currPage == 1) {
            prevButton.interactable = false;
            nextButton.interactable = true;
        }
        else if (currPage == maxPage){
            prevButton.interactable = true;
            nextButton.interactable = false;
        }
        else {
            prevButton.interactable = true;
            nextButton.interactable = true;
        }
        if (prevPage > 0) Pages[prevPage - 1].SetActive(false);
        Pages[currPage - 1].SetActive(true);
    }
    private void SavePage()
    {
        PlayerPrefs.SetInt(SavedData.CurrPage, currPage);
        PlayerPrefs.Save();
    }
}
