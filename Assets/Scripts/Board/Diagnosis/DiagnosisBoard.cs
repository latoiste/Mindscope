using System.Collections.Generic;
using DG.Tweening;
using TMPro;
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

    private void SetSortingLayerName(string layerName)
    {
        int layerId = SortingLayer.NameToID(layerName);

        var sprites = GetComponentsInChildren<SpriteRenderer>(true);
        var textMeshPros = GetComponentsInChildren<TextMeshPro>(true);

        foreach (var s in sprites) s.sortingLayerID = layerId;
        foreach (var t in textMeshPros) t.sortingLayerID = layerId;
    }
    
    protected override async void OnBoardMoved(Board board, bool opening)
    {
        if (opening)
        {
            SetSortingLayerName("openingBoard");
            await movingOp;
            SetSortingLayerName("board");
            sprite.sortingOrder = 0;
        } else
        {
            SetSortingLayerName("closingBoard");
            await movingOp;
            SetSortingLayerName("board");
            sprite.sortingOrder = 5;
        }
    }
}
