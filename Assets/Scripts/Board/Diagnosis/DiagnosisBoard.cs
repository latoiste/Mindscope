using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class DiagnosisBoard : Board
{
    [SerializeField] private GameObject initialPageGo;
    [SerializeField] private GameObject diseasePageGo;
    [SerializeField] private GameManager gameManager;

    private InitialPage initialPage;
    private DiseasePage diseasePage;
    private GameObject currentPage;
    private Dictionary<string, GameObject> pageMap;
    public UnityEvent<string> onDiagnosed;

    protected override void Awake()
    {
        base.Awake();

        initialPage = initialPageGo.GetComponent<InitialPage>();
        diseasePage = diseasePageGo.GetComponent<DiseasePage>();
        
        toggleButton.Disable();
        currentPage = initialPageGo;

        pageMap = new()
        {
            { "initial", initialPageGo },
            { "disease", diseasePageGo },
        };

        initialPage.onDiagnosed.AddListener(EndGame);
        diseasePage.onDiagnosed.AddListener(EndGame);

        initialPageGo.SetActive(true);
        diseasePageGo.SetActive(false);
    }

    public async void EnableBoard()
    {
        await transform
            .DOLocalMoveX(1.8f, 1f)
            .SetEase(Ease.OutCubic)
            .AsyncWaitForCompletion();

        originalCanvasPos = transform.position;
        toggleButton.Enable();
    }

    public void SwitchPage(string pageId)
    {
        GameObject page = pageMap[pageId];
        if (page == currentPage) return;
        
        page.SetActive(true);
        currentPage.SetActive(false);

        currentPage = page;
    }

    private void EndGame(string outcome)
    {
        if (outcome.Length == 0) return;
        gameManager.Diagnose(outcome);
    }
    
    protected override void OnBoardMoved(Board board, bool opening)
    {
        return;
    }
}
