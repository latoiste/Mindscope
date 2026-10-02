using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class Button : MonoBehaviour
{
    public UnityEvent onClick;

    // Changed from 'private' to 'protected' so Checkbox can access it
    protected BoxCollider2D boxCollider;

    protected virtual void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public void Enable()
    {
        if (boxCollider != null) boxCollider.enabled = true;
    }

    public void Disable()
    {
        if (boxCollider != null) boxCollider.enabled = false;
    }

    // Changed to 'protected virtual' so Checkbox can override it
    protected virtual void OnMouseDown()
    {
        if (boxCollider != null && boxCollider.enabled)
        {
            onClick?.Invoke();
        }
    }

    protected virtual void OnDestroy()
    {
        onClick.RemoveAllListeners();
    }
}