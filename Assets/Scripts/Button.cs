using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class Button : MonoBehaviour
{
    public UnityEvent onClick;

    private BoxCollider2D boxCollider;

    protected virtual void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public void Enable()
    {
        boxCollider.enabled = true;
    }

    public void Disable()
    {
        boxCollider.enabled = false;
    }

    void OnMouseDown()
    {
        onClick?.Invoke();
    }

    void OnDestroy()
    {
        onClick.RemoveAllListeners();
    }
}