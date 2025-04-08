using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PreviousAndNextButtonUI : MonoBehaviour
{
    [SerializeField] Button nextButton;
    [SerializeField] Button prevButton;
    [SerializeField] GameObject[] Pages;
    private int currPage;
    private int prevPage = 0;
    private int maxPage;
    private void Start()
    {
        nextButton.onClick.RemoveAllListeners();
        prevButton.onClick.RemoveAllListeners();

        nextButton.onClick.AddListener(NextPage);
        prevButton.onClick.AddListener(PrevPage);

        for (int i = 0; i < maxPage; i++) Pages[i].SetActive(false);

        currPage = PlayerPrefs.GetInt(SavedData.CurrPage, 1);
        maxPage = Pages.Length;
        
        LoadPage();
    }
    private void NextPage()
    {
        if (currPage < maxPage) {
            prevPage = currPage;
            currPage++;
            SavePage();
            LoadPage();
        }
    }
    private void PrevPage()
    {
        if (currPage > 1) {
            prevPage = currPage;
            currPage--;
            SavePage();
            LoadPage();
        }
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
