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
    public UnityEvent<string> onDiagnosed = new();

    protected override void Awake()
    {
        // 1. Prevent toggleButton from accidentally stealing a button from the child pages
        if (toggleButton == null)
        {
            Button[] allButtons = GetComponentsInChildren<Button>(true);
            foreach (var btn in allButtons)
            {
                bool isInsideInitial = initialPageGo != null && btn.transform.IsChildOf(initialPageGo.transform);
                bool isInsideDisease = diseasePageGo != null && btn.transform.IsChildOf(diseasePageGo.transform);

                if (!isInsideInitial && !isInsideDisease)
                {
                    toggleButton = btn;
                    break;
                }
            }
        }

        // 2. Isolate pages so they never render together
        if (initialPageGo != null) initialPageGo.SetActive(true);
        if (diseasePageGo != null) diseasePageGo.SetActive(false);

        base.Awake();

        if (initialPageGo != null) initialPage = initialPageGo.GetComponent<InitialPage>();
        if (diseasePageGo != null) diseasePage = diseasePageGo.GetComponent<DiseasePage>();

        // Disable toggle tab until questions are done
        if (toggleButton != null) toggleButton.Disable();

        currentPage = initialPageGo;

        pageMap = new()
        {
            { "initial", initialPageGo },
            { "disease", diseasePageGo },
        };

        if (initialPage != null)
        {
            if (initialPage.onDiagnosed == null) initialPage.onDiagnosed = new();
            initialPage.onDiagnosed.AddListener(EndGame);
        }

        if (diseasePage != null)
        {
            if (diseasePage.onDiagnosed == null) diseasePage.onDiagnosed = new();
            diseasePage.onDiagnosed.AddListener(EndGame);
        }
    }

    public void EnableBoard()
    {
        transform
            .DOLocalMoveX(1.8f, 1f)
            .SetEase(Ease.OutCubic)
            .OnComplete(() =>
            {
                originalCanvasPos = transform.position;
                if (toggleButton != null) toggleButton.Enable();
            });
    }

    public void SwitchPage(string pageId)
    {
        if (pageMap == null || !pageMap.ContainsKey(pageId)) return;

        GameObject page = pageMap[pageId];
        if (page == null) return;

        page.SetActive(true);
        if (currentPage != null && currentPage != page)
        {
            currentPage.SetActive(false);
        }

        currentPage = page;
    }

    private void EndGame(string outcome)
    {
        if (string.IsNullOrEmpty(outcome)) return;
        if (gameManager != null)
        {
            gameManager.Diagnose(outcome);
        }
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
            if (movingOp != null) await movingOp;
            SetSortingLayerName("board");
            if (sprite != null) sprite.sortingOrder = 0;
        }
        else
        {
            SetSortingLayerName("closingBoard");
            if (movingOp != null) await movingOp;
            SetSortingLayerName("board");
            if (sprite != null) sprite.sortingOrder = 5;
        }
    }
}