using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

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

        if (prevArrow != null)
        {
            prevArrow.onClick.AddListener(() => NextPage(-1));
            prevArrowSprite = prevArrow.gameObject.GetComponentInChildren<SpriteRenderer>();
        }

        if (nextArrow != null)
        {
            nextArrow.onClick.AddListener(() => NextPage(1));
            nextArrowSprite = nextArrow.gameObject.GetComponentInChildren<SpriteRenderer>();
        }

        LoadPageData();

        if (pages != null && pages.Length > 0)
        {
            SetPageContent(0);
            UpdateArrowSprite();
        }
    }

    private void LoadPageData()
    {
        TextAsset jsonAsset = Resources.Load<TextAsset>("informationPage");

        if (jsonAsset == null)
        {
            Debug.LogError("[InformationBoard] Could not find 'informationPage.json' in Resources!");
            return;
        }

        InformationPageDataWrapper wrapper = JsonUtility.FromJson<InformationPageDataWrapper>(jsonAsset.text);

        if (wrapper == null || wrapper.pages == null || wrapper.pages.Length == 0)
        {
            Debug.LogError("[InformationBoard] informationPage.json is empty or formatted incorrectly!");
            return;
        }

        pages = wrapper.pages;
    }

    protected override async void OnBoardMoved(Board board, bool opening)
    {
        if (opening)
        {
            SetSortingLayerName("openingBoard");
            await movingOp;
            SetSortingLayerName("board");
            if (sprite != null) sprite.sortingOrder = 0;
        }
        else
        {
            SetSortingLayerName("closingBoard");
            await movingOp;
            SetSortingLayerName("board");
            if (sprite != null) sprite.sortingOrder = 5;
        }
    }

    private void SetSortingLayerName(string layerName)
    {
        int layerId = SortingLayer.NameToID(layerName);

        if (sprite != null) sprite.sortingLayerID = layerId;
        if (conditionDescription != null) conditionDescription.sortingLayerID = layerId;
        if (conditionName != null) conditionName.sortingLayerID = layerId;
        if (prevArrowSprite != null) prevArrowSprite.sortingLayerID = layerId;
        if (nextArrowSprite != null) nextArrowSprite.sortingLayerID = layerId;
    }

    private void NextPage(int step)
    {
        if (pages == null || pages.Length == 0) return;

        int newPageIndex = pageIndex + step;
        if (newPageIndex < 0 || newPageIndex > (pages.Length - 1)) return;

        pageIndex = newPageIndex;
        SetPageContent(newPageIndex);
        UpdateArrowSprite();
    }

    private void UpdateArrowSprite()
    {
        if (pages == null || pages.Length == 0) return;

        if (prevArrowSprite != null)
        {
            Color c = prevArrowSprite.color;
            c.a = pageIndex == 0 ? 0.3f : 1f;
            prevArrowSprite.color = c;
        }

        if (nextArrowSprite != null)
        {
            Color c = nextArrowSprite.color;
            c.a = pageIndex == pages.Length - 1 ? 0.3f : 1f;
            nextArrowSprite.color = c;
        }
    }

    private void SetPageContent(int index)
    {
        if (pages == null || index < 0 || index >= pages.Length) return;

        InformationPageData page = pages[index];
        if (conditionName != null) conditionName.text = page.name;
        if (conditionDescription != null) conditionDescription.text = page.description;
    }
}