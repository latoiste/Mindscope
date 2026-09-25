using System;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    public UnityEvent onClick;

    private BoxCollider2D boxCollider;

    void Awake()
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