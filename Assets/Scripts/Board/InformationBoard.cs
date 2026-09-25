using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class InformationBoard : Board
{
    private int pageIndex;
    private InformationPageData[] pages;

    [SerializeField] private TextMeshPro conditionName;
    [SerializeField] private TextMeshPro conditionDescription;
    [SerializeField] private Button prevArrow; 
    [SerializeField] private Button nextArrow;

    private SpriteRenderer prevArrowSprite;
    private SpriteRenderer nextArrowSprite;

    protected override void Awake()
    {
        base.Awake();
        
        pageIndex = 0;
        
        prevArrow.onClick.AddListener(() => NextPage(-1));
        prevArrowSprite = prevArrow.gameObject.GetComponentInChildren<SpriteRenderer>();
        
        nextArrow.onClick.AddListener(() => NextPage(1));
        nextArrowSprite = nextArrow.gameObject.GetComponentInChildren<SpriteRenderer>();
        
        string filepath = @"Assets/Storage/informationPage.json";

        string json = File.ReadAllText(filepath);

        InformationPageDataWrapper pageDatas = JsonUtility.FromJson<InformationPageDataWrapper>(json);
        Debug.Log(pageDatas.pages.Length);

        if (pageDatas.pages.Length == 0) Debug.LogError("InformationBoard page data empty");
        pages = pageDatas.pages;

        SetPageContent(0);
        UpdateArrowSprite();
    }

    protected override async void OnBoardMoved(Board board, bool opening)
    {
        if (opening)
        {
            sprite.sortingOrder = 0;
        } else
        {
            await movingOp;
            sprite.sortingOrder = 5;
        }
    }

    private void NextPage(int step)
    {
        int newPageIndex = pageIndex + step;

        if (newPageIndex < 0 || newPageIndex > (pages.Length - 1)) return;

        pageIndex = newPageIndex;

        SetPageContent(newPageIndex);
        UpdateArrowSprite();
    }

    private void UpdateArrowSprite()
    {
        Color prevArrowColor = prevArrowSprite.color;
        Color nextArrowColor = nextArrowSprite.color;

        prevArrowColor.a = pageIndex == 0 ? 0.5f : 1f;
        nextArrowColor.a = pageIndex == pages.Length - 1 ? 0.5f : 1f;

        prevArrowSprite.color = prevArrowColor;
        nextArrowSprite.color = nextArrowColor;
    }

    private void SetPageContent(int pageIndex)
    {
        InformationPageData page = pages[pageIndex];
        
        conditionName.text = page.name;
        conditionDescription.text = page.description;
    }
}