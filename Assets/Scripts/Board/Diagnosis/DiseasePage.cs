using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DiseasePage : MonoBehaviour
{
    [SerializeField] private Button diagnoseButton;
    [SerializeField] private TextMeshPro diseaseName;
    [SerializeField] private Animator diseaseSpriteAnim;
    [SerializeField] private Button prevArrow;
    [SerializeField] private Button nextArrow;
    private DiagnosisBoard diagnosisBoard;
    private SpriteRenderer nextArrowSprite;
    private int pageIndex;
    private DiagnosisPageData[] pages;
    public UnityEvent<string> onDiagnosed;

    void Awake()
    {
        pageIndex = 0;
        diagnosisBoard = GetComponentInParent<DiagnosisBoard>();

        diagnoseButton.onClick.AddListener(Diagnose);
        prevArrow.onClick.AddListener(() => NextDiseasePage(-1));
        nextArrow.onClick.AddListener(() => NextDiseasePage(1));
        nextArrowSprite = nextArrow.gameObject.GetComponentInChildren<SpriteRenderer>();
        
        string filepath = @"Assets/Storage/diagnosisPage.json";
        string json = File.ReadAllText(filepath);

        DiagnosisPageDataWrapper pageDatas = JsonUtility.FromJson<DiagnosisPageDataWrapper>(json);

        if (pageDatas.pages.Length == 0) Debug.LogWarning("DiagnosisBoard page data empty");
        pages = pageDatas.pages;
    }

    private void Diagnose()
    {
        if (!gameObject.activeSelf) return;
        
        DiagnosisPageData page = pages[pageIndex];

        onDiagnosed.Invoke(page.value);
    }
    
    private void NextDiseasePage(int step)
    {
        int newPageIndex = pageIndex + step;

        if (newPageIndex < 0)
        {
            diagnosisBoard.SwitchPage("initial");
            return;
        }

        if (newPageIndex > (pages.Length - 1)) return;

        pageIndex = newPageIndex;

        SetPageContent(newPageIndex);
        UpdateArrowSprite();
    }

    private void UpdateArrowSprite()
    {
        Color nextArrowColor = nextArrowSprite.color;

        nextArrowColor.a = pageIndex == pages.Length - 1 ? 0.5f : 1f;
        nextArrowSprite.color = nextArrowColor;
    }

    private void SetPageContent(int pageIndex)
    {
        DiagnosisPageData page = pages[pageIndex];

        diseaseSpriteAnim.Play(page.spriteId);
        diseaseName.text = page.name;
    }
}