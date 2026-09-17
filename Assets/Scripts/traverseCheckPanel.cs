using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class traversepage : MonoBehaviour
{
    public Animator animator;
    public GameObject leftArrow,  leftArrowButton;
    public GameObject rightArrow, rightArrowButton;
    
    public traversepage otherArrow;
    public bool isLeftArrow;
    private int currPage;

    public TextMeshProUGUI title;
    private string[] titles;

    public int firstPage, lastPage;
    private GameObject[] pages;
    public void Start()
    {
        currPage = 0;
        pages = new GameObject[lastPage + 1];

        for (int i = 0; i <= lastPage; i++)
        {
            pages[i] = GameObject.Find("secondPanelCanvas/pages/page " + i);
            if (i > 0) pages[i].SetActive(false);
        }

        pages[0].SetActive(true);

        titles = new string[lastPage + 1];
        titles[0] = "THE HOLLOW\n(MAJOR DEPRESSION DISORDER)";
        titles[1] = "THE SIGNAL\n(GENERALIZED ANXIETY DISORDER)";
        titles[2] = "THE REMNANT\n(POST-TRAUMATIC STRESS DISORDER)";

        otherArrow.titles = titles;

        if (currPage - 1 < firstPage)
        {
            leftArrow.SetActive(false);
            leftArrowButton.SetActive(false);
        }

        if (currPage + 1 > lastPage)
        {
            rightArrow.SetActive(false);
            rightArrowButton.SetActive(false);
        }

        title.text = titles[currPage];
    }

    public void traverse()
    {

        currPage = otherArrow.currPage;

        if (isLeftArrow && currPage - 1 >= firstPage)
        {
            currPage--;
            animator.SetInteger("pageNum", currPage);

            if (currPage - 1 < firstPage)
            {
                leftArrow.SetActive(false);
                leftArrowButton.SetActive(false);
            }

            if (currPage + 1 <= lastPage)
            {
                rightArrow.SetActive(true);
                rightArrowButton.SetActive(true);
            }
        }

        if (!isLeftArrow && currPage + 1 <= lastPage)
        {
            currPage++;
            animator.SetInteger("pageNum", currPage);
            if (currPage + 1 > lastPage)
            {
                rightArrow.SetActive(false);
                rightArrowButton.SetActive(false);
            }

            if (currPage - 1 >= firstPage)
            {
                leftArrow.SetActive(true);
                leftArrowButton.SetActive(true);
            }
        }

        pages[currPage].SetActive(true);
        if (currPage - 1 >= 0)
        {
            pages[currPage - 1].SetActive(false);
        }

        if (currPage + 1 <= lastPage)
        {
            pages[currPage + 1].SetActive(false);
        }

        otherArrow.currPage = currPage;
        title.text = titles[currPage];
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

}
