using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DiagnosisBoard : Board
{
    [SerializeField] private TextMeshPro diseaseName;
    [SerializeField] private Animator diseaseSpriteAnim;
    [SerializeField] private List<SpriteEntry> diseaseSprites;
    [SerializeField] private Button prevArrow;
    [SerializeField] private Button nextArrow;
    [SerializeField] private GameObject initialPage;
    [SerializeField] private GameObject diseasePage;
    private SpriteRenderer prevArrowSprite;    
    private SpriteRenderer nextArrowSprite;    
    private DiagnosisPageData[] pages;
    private int pageIndex;

    protected override void Awake()
    {
        base.Awake();
        
        pageIndex = 0;
        
        prevArrow.onClick.AddListener(() => NextDiseasePage(-1));
        prevArrowSprite = prevArrow.gameObject.GetComponentInChildren<SpriteRenderer>();
        
        nextArrow.onClick.AddListener(() => NextDiseasePage(1));
        nextArrowSprite = nextArrow.gameObject.GetComponentInChildren<SpriteRenderer>();
        
        string filepath = @"Assets/Storage/diagnosisPage.json";

        string json = File.ReadAllText(filepath);

        DiagnosisPageDataWrapper pageDatas = JsonUtility.FromJson<DiagnosisPageDataWrapper>(json);

        if (pageDatas.pages.Length == 0) Debug.LogError("DiagnosisBoard page data empty");
        pages = pageDatas.pages;

        SetPageContent(0);
        UpdateArrowSprite();
        toggleButton.Disable();
    }

    public async void EnableBoard()
    {
        await transform
            .DOLocalMoveX(1.8f, 1f)
            .SetEase(Ease.OutCubic)
            .AsyncWaitForCompletion();

        originalCanvasPos = transform.position;
        diseasePage.SetActive(false);
        toggleButton.Enable();
    }

    private void NextDiseasePage(int step)
    {
        int newPageIndex = pageIndex + step;

        if (newPageIndex < 0)
        {
            
            return;
        }

        if (newPageIndex > (pages.Length - 1)) return;

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
        DiagnosisPageData page = pages[pageIndex];

        diseaseSpriteAnim.Play(page.spriteId);
        diseaseName.text = page.name;
    }
    
    protected override void OnBoardMoved(Board board, bool opening)
    {
        return;
    }
}
