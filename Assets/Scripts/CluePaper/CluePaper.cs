using TMPro;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CluePaper : MonoBehaviour
{
    private TextMeshPro textMeshPro;
    private Bounds bounds;
    private RectTransform clipboardBoundary;
    private Vector2 mouseOffset;
    private SpriteRenderer sprite;
    private ClueBoard clueBoard;
    private BoxCollider2D boxCollider;
    private bool isInitialized = false;

    void Awake()
    {
        EnsureInitialized();
    }

    private void EnsureInitialized()
    {
        if (isInitialized) return;

        boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider != null) bounds = boxCollider.bounds;

        sprite = GetComponentInChildren<SpriteRenderer>();
        textMeshPro = GetComponentInChildren<TextMeshPro>();
        clueBoard = GetComponentInParent<ClueBoard>();

        isInitialized = true;
    }

    public void Init(string text, RectTransform clipboardBoundary, int sortingOrder)
    {
        EnsureInitialized();

        this.clipboardBoundary = clipboardBoundary;

        if (textMeshPro != null)
        {
            textMeshPro.text = text;
        }

        SetSortingOrder(sortingOrder);
    }

    public void SetSortingOrder(int sortingOrder)
    {
        EnsureInitialized();

        if (sprite != null) sprite.sortingOrder = sortingOrder * 10;
        if (textMeshPro != null) textMeshPro.sortingOrder = sortingOrder * 10 + 1;
    }

    void OnMouseDown()
    {
        EnsureInitialized();

        mouseOffset = (Vector2)transform.position - MousePosition();

        if (clueBoard != null)
        {
            clueBoard.BringClueToFront(this);
        }
    }

    void OnMouseDrag()
    {
        EnsureInitialized();

        if (clipboardBoundary == null) return;

        Vector2 newPos = MousePosition() + mouseOffset;
        Bounds clipboardBounds = GetBounds();

        float halfWidth = bounds.extents.x;
        float halfHeight = bounds.extents.y;

        float minX = clipboardBounds.min.x;
        float maxX = clipboardBounds.max.x;
        float minY = clipboardBounds.min.y;
        float maxY = clipboardBounds.max.y;

        float x = Mathf.Clamp(newPos.x, minX + halfWidth, maxX - halfWidth);
        float y = Mathf.Clamp(newPos.y, minY + halfHeight, maxY - halfHeight);

        transform.position = new Vector2(x, y);
    }

    private Vector2 MousePosition()
    {
        Camera cam = Camera.main;
        if (cam == null) cam = FindFirstObjectByType<Camera>();
        if (cam == null) return transform.position;

        return cam.ScreenToWorldPoint(Input.mousePosition);
    }

    private Bounds GetBounds()
    {
        Vector3[] corners = new Vector3[4];
        clipboardBoundary.GetWorldCorners(corners);

        Bounds b = new Bounds(corners[0], Vector3.zero);
        for (int i = 1; i < 4; i++) b.Encapsulate(corners[i]);

        return b;
    }
}