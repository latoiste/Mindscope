using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

[RequireComponent(typeof(BoxCollider2D))]
public class CluePaper : MonoBehaviour
{
    private TextMeshPro textMeshPro;
    private Bounds bounds;
    private RectTransform clipboardBoundary;
    private Vector2 mouseOffset;
    private SpriteRenderer sprite;

    void Awake()
    {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        
        bounds = boxCollider.bounds;
        sprite = GetComponentInChildren<SpriteRenderer>();
        textMeshPro = GetComponentInChildren<TextMeshPro>();
    }

    public void Init(string text, RectTransform clipboardBoundary, int sortingOrder)
    {
        textMeshPro.text = text;
        this.clipboardBoundary = clipboardBoundary;

        sprite.sortingOrder = sortingOrder * 10;
        textMeshPro.sortingOrder = sortingOrder * 10 + 1;
    }

    void OnMouseDown()
    {
        mouseOffset = new Vector2(transform.position.x, transform.position.y) - MousePosition();
    }

    void OnMouseDrag()
    {
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
        Vector3 screenPosition = Input.mousePosition; 

        return Camera.main.ScreenToWorldPoint(new Vector2(screenPosition.x, screenPosition.y));   
    }

    private Bounds GetBounds()
    {
        Vector3[] corners = new Vector3[4];
        clipboardBoundary.GetWorldCorners(corners);

        Bounds bounds = new Bounds(corners[0], Vector3.zero);

        for (int i = 1; i < 4; i++) bounds.Encapsulate(corners[i]);

        return bounds;
    }
}