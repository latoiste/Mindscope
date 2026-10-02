using UnityEngine;
using UnityEngine.Events;

public class Checkbox : Button
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Sprite checkedSprite;
    [SerializeField] private Sprite uncheckedSprite;
    [SerializeField] public string value;

    private bool isChecked = false;
    public bool IsChecked => isChecked;

    public UnityEvent onChecked;

    protected override void Awake()
    {
        base.Awake();
        SetChecked(false);
    }

    protected override void OnMouseDown()
    {
        if (boxCollider == null || !boxCollider.enabled) return;

        // 1. ALWAYS toggle state FIRST before any listener reads it
        SetChecked(!isChecked);
        onChecked?.Invoke();

        // 2. Base method invokes onClick safely
        base.OnMouseDown();
    }

    public void SetChecked(bool checkedValue)
    {
        isChecked = checkedValue;
        if (sprite != null)
        {
            sprite.sprite = isChecked ? checkedSprite : uncheckedSprite;
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        onChecked.RemoveAllListeners();
    }
}