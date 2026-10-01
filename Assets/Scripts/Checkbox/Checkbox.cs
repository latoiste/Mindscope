using UnityEngine;
using UnityEngine.Events;

public class Checkbox : Button
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Sprite checkedSprite;
    [SerializeField] private Sprite uncheckedSprite;
    [SerializeField] public string value;

    private bool isChecked;
    public bool IsChecked => isChecked; // <--- ADD THIS GETTER

    public UnityEvent onChecked;

    protected override void Awake()
    {
        base.Awake();

        isChecked = false;
        onClick.AddListener(ToggleCheck);
    }

    private void ToggleCheck()
    {
        isChecked = !isChecked;
        if (sprite != null)
        {
            sprite.sprite = isChecked ? checkedSprite : uncheckedSprite;
        }
        onChecked?.Invoke();
    }

    public void SetChecked(bool checkedValue)
    {
        isChecked = checkedValue;
        if (sprite != null)
        {
            sprite.sprite = isChecked ? checkedSprite : uncheckedSprite;
        }
    }

    void OnDestroy()
    {
        onClick.RemoveAllListeners();
        onChecked.RemoveAllListeners();
    }
}