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

        TextAsset jsonAsset = Resources.Load<TextAsset>("diagnosisPage");
        if (jsonAsset == null)
        {
            Debug.LogError("[DiseasePage] Could not find 'diagnosisPage.json' in Resources!");
            return;
        }

        DiagnosisPageDataWrapper pageDatas = JsonUtility.FromJson<DiagnosisPageDataWrapper>(jsonAsset.text);
        if (pageDatas == null || pageDatas.pages == null || pageDatas.pages.Length == 0)
        {
            Debug.LogWarning("[DiseasePage] DiagnosisBoard page data empty!");
            return;
        }

        pages = pageDatas.pages;
        SetPageContent(0);
        UpdateArrowSprite();
    }

    void OnEnable()
    {
        // Diagnose button must ALWAYS be available on the disease pages
        if (diagnoseButton != null)
        {
            diagnoseButton.gameObject.SetActive(true);
        }
    }

    private void Diagnose()
    {
        if (!gameObject.activeSelf || pages == null || pages.Length == 0) return;

        DiagnosisPageData page = pages[pageIndex];
        onDiagnosed?.Invoke(page.value);
    }

    private void NextDiseasePage(int step)
    {
        if (pages == null || pages.Length == 0) return;

        int newPageIndex = pageIndex + step;

        // If clicking back on the first disease, go back to initial page
        if (newPageIndex < 0)
        {
            diagnosisBoard.SwitchPage("initial");
            return;
        }

        if (newPageIndex >= pages.Length) return;

        pageIndex = newPageIndex;
        SetPageContent(newPageIndex);
        UpdateArrowSprite();
    }

    private void UpdateArrowSprite()
    {
        if (nextArrowSprite == null || pages == null) return;

        Color nextArrowColor = nextArrowSprite.color;
        nextArrowColor.a = pageIndex == pages.Length - 1 ? 0.5f : 1f;
        nextArrowSprite.color = nextArrowColor;
    }

    private void SetPageContent(int index)
    {
        if (pages == null || index < 0 || index >= pages.Length) return;

        DiagnosisPageData page = pages[index];
        if (diseaseSpriteAnim != null) diseaseSpriteAnim.Play(page.spriteId);
        if (diseaseName != null) diseaseName.text = page.name;
    }
}