using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class ClueBoard : Board
{
    [SerializeField] private RectTransform clipboardBoundary;
    [SerializeField] private CluePaper cluePaperPrefab;
    [SerializeField] private SpriteRenderer newClueNotif;


    private Vector2 newClueNotifPos; 
    private int cluePaperCount;

    protected override void Awake()
    {
        base.Awake();
        
        cluePaperCount = 0;
        newClueNotifPos = newClueNotif.transform.position;
        newClueNotif.enabled = false;
    }

    protected override async void OnBoardMoved(Board _, bool opening)
    {
        sprite.sortingOrder = opening ? 0 : 100;

        if (opening && newClueNotif.enabled) newClueNotif.enabled = false; 
    }

    public void ShowClue(string text)
    {
        CluePaper cluePaper = Instantiate(cluePaperPrefab, clipboardBoundary.transform.position, transform.rotation, transform);
        cluePaper.Init(text, clipboardBoundary, cluePaperCount);
        cluePaperCount++;

        if (!moved) _ = ShowNotification();
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