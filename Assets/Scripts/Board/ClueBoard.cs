using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class ClueBoard : Board
{
    [SerializeField] private RectTransform clipboardBoundary;
    [SerializeField] private CluePaper cluePaperPrefab;
    [SerializeField] private SpriteRenderer newClueNotif;

    private List<CluePaper> cluePapers;
    private Vector2 newClueNotifPos; 

    protected override void Awake()
    {
        base.Awake();
        
        cluePapers = new();
        newClueNotifPos = newClueNotif.transform.position;
        newClueNotif.enabled = false;

        var clueProfile = GetComponentInChildren<CluePaper>();
        clueProfile.Init(
            "Name: Hal\nAge: 21\nOccupation: Retail worker\nReason for visiting: Feeling tired constantly",
            clipboardBoundary,
            0
        );
        cluePapers.Add(clueProfile);
    }

    protected override async void OnBoardMoved(Board _, bool opening)
    {
        if (opening)
        {
            SetSortingLayerName("openingBoard");
            await movingOp;
            SetSortingLayerName("openedClueBoard");
            sprite.sortingOrder = 0;
        } else
        {
            SetSortingLayerName("closingBoard");
            await movingOp;
            SetSortingLayerName("board");
            sprite.sortingOrder = 100;
        }

        if (opening && newClueNotif.enabled) newClueNotif.enabled = false; 
    }

    private void SetSortingLayerName(string layerName)
    {
        int layerId = SortingLayer.NameToID(layerName);

        sprite.sortingLayerID = layerId;
    }

    public void ShowClue(string text)
    {
        CluePaper cluePaper = Instantiate(cluePaperPrefab, clipboardBoundary.transform.position, transform.rotation, transform);
        cluePaper.Init(text, clipboardBoundary, cluePapers.Count);

        if (!moved) _ = ShowNotification();
        cluePapers.Add(cluePaper);
    }

    public void BringClueToFront(CluePaper cluePaper)
    {
        cluePapers.Remove(cluePaper);
        cluePapers.Add(cluePaper);

        for (int i = 0; i < cluePapers.Count; i++)
        {
            cluePapers[i].SetSortingOrder(i);
        }
    }

    private async Task ShowNotification()
    {
        newClueNotif.enabled = true;

        await newClueNotif.transform
            .DOMoveY(newClueNotifPos.y + 0.5f, 0.1f)
            .AsyncWaitForCompletion();

        await newClueNotif.transform
            .DOMoveY(newClueNotifPos.y, 0.1f)
            .AsyncWaitForCompletion();
    }
}